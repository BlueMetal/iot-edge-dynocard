using System;
using System.Linq;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using System.Collections.Generic;     // for KeyValuePair<>
using Microsoft.Azure.Devices.Shared; // for TwinCollection
using Newtonsoft.Json;                // for JsonConvert
using Sql = System.Data.SqlClient;
using DynoCardAlertModule.Model;
using DynoCardAlertModule.Config;

namespace DynoCardAlertModule
{    class Program
    {
        private static int counter;
        private static string _sqlConnectionString;

        static void Main(string[] args)
        {
            // The Edge runtime gives us the connection string we need -- it is injected as an environment variable
            string connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");

            // Cert verification is not yet fully functional when using Windows OS for the container
            bool bypassCertVerification = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (!bypassCertVerification) InstallCert();
            Init(connectionString, bypassCertVerification).Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        private  static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Add certificate in local cert store for use by client for secure connection to IoT Edge runtime
        /// </summary>
        private static void InstallCert()
        {
            string certPath = Environment.GetEnvironmentVariable("EdgeModuleCACertificateFile");
            if (string.IsNullOrWhiteSpace(certPath))
            {
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing path to certificate file.");
            }
            else if (!File.Exists(certPath))
            {
                // We cannot proceed further without a proper cert file
                Console.WriteLine($"Missing path to certificate collection file: {certPath}");
                throw new InvalidOperationException("Missing certificate file.");
            }
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(certPath)));
            Console.WriteLine("Added Cert: " + certPath);
            store.Close();
        }


        /// <summary>
        /// Initializes the DeviceClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        private static async Task Init(string connectionString, bool bypassCertVerification = false)
        {
            Console.WriteLine("Connection String {0}", connectionString);

            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            
            // During dev you might want to bypass the cert verification. It is highly recommended to verify certs systematically in production
            if (bypassCertVerification)
            {
                mqttSetting.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }
            
            ITransportSettings[] settings = { mqttSetting };

            // Open a connection to the Edge runtime
            DeviceClient ioTHubModuleClient = DeviceClient.CreateFromConnectionString(connectionString, settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initializing...");

           // Read Module Twin Desired Properties
            var moduleTwin = await ioTHubModuleClient.GetTwinAsync();
            var desiredProperties = moduleTwin.Properties.Desired;

            string json = JsonConvert.SerializeObject(desiredProperties);
            ModbusLayoutConfig modbusConfig = JsonConvert.DeserializeObject<ModbusLayoutConfig>(json);
            ModbusMessage.LayoutConfig = modbusConfig;

            _sqlConnectionString = desiredProperties["sqlConnectionString"];
            Console.WriteLine($"sqlConnectionString: {_sqlConnectionString}");

            try
            {
                Console.WriteLine("Setting module twin property handler");
                // Attach callback for Twin desired properties updates
                await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);

                Console.WriteLine("Setting modbusInput handler");
                // Register callback to be called when a message is received by the module
                await ioTHubModuleClient.SetInputMessageHandlerAsync("modbusInput", ProcessModbusInput, ioTHubModuleClient);

                Console.WriteLine("Setting opcInput handler");
                // Register callback to be called when a message is received by the module
                await ioTHubModuleClient.SetInputMessageHandlerAsync("opcInput", ProcessOPCInput, ioTHubModuleClient);

                Console.WriteLine("Setting alertInput handler");
                // Register callback to be called when an alert is received by the module
                await ioTHubModuleClient.SetInputMessageHandlerAsync("alertInput", ProcessAlert, ioTHubModuleClient);

                Console.WriteLine("Done setting inputs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during Init: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                string json = JsonConvert.SerializeObject(desiredProperties);
                Console.WriteLine("Desired property change:");

                ModbusLayoutConfig modbusConfig = JsonConvert.DeserializeObject<ModbusLayoutConfig>(json);
                ModbusMessage.LayoutConfig = modbusConfig;

                _sqlConnectionString = desiredProperties["sqlConnectionString"];
                Console.WriteLine($"sqlConnectionString: {_sqlConnectionString}");
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error when receiving desired property: {0}", exception);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error when receiving desired property: {0}", ex.Message);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called whenever the module is sent a message from the EdgeHub. 
        /// It just pipe the messages without any change.
        /// It prints all the incoming messages.
        /// </summary>
        private static async Task<MessageResponse> ProcessModbusInput(Message message, object userContext)
        {
            Console.WriteLine("Processing modbus input");

            var counterValue = Interlocked.Increment(ref counter);

            try
            {
                DeviceClient deviceClient = (DeviceClient)userContext;
                var modbusMessage = new ModbusMessage(message);
                var dynoCard = modbusMessage.ToDynoCard();

                string json = JsonConvert.SerializeObject(dynoCard);
                System.Console.WriteLine(json);

                await PersistDynoCard(dynoCard);
                
                var dynoCardMessage = dynoCard.ToDeviceMessage();
                await deviceClient.SendEventAsync("output1", dynoCardMessage);

                // Indicate that the message treatment is completed
                return MessageResponse.Completed;
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error in sample: {0}", exception);
                }
                // Indicate that the message treatment is not completed
                var deviceClient = (DeviceClient)userContext;
                return MessageResponse.Abandoned;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
                // Indicate that the message treatment is not completed
                //DeviceClient deviceClient = (DeviceClient)userContext;
                return MessageResponse.Abandoned;
            }
        }

        private static async Task<MessageResponse> ProcessOPCInput(Message message, object userContext)
        {
            Console.WriteLine("In the Filter Message handler");

            var counterValue = Interlocked.Increment(ref counter);
            await Task.FromResult(true);

            return MessageResponse.Completed;
        }
       
        private static async Task<MessageResponse> ProcessAlert(Message message, object userContext)
        {
            Console.WriteLine("Processing an alert");

            var counterValue = Interlocked.Increment(ref counter);

            try
            {
                DeviceClient deviceClient = (DeviceClient)userContext;

                var messageBytes = message.GetBytes();
                var messageString = Encoding.UTF8.GetString(messageBytes);
                Console.WriteLine($"Received alert {counterValue}: [{messageString}]");

                if (!string.IsNullOrEmpty(messageString))
                {
                    var anomaly = JsonConvert.DeserializeObject<DynoCardAnomaly>(messageString);
                    var previousCardList = GetPreviousCards(anomaly);
                    
                    var previousCardsMessgeBytes = JsonConvert.SerializeObject(previousCardList);
                    var previousCardsMessgeBytesString = Encoding.UTF8.GetBytes(previousCardsMessgeBytes);
                    var previousCardsMessage = new Message(previousCardsMessgeBytesString);

                    Console.WriteLine("Sending Alert");
                    previousCardsMessage.Properties["MessageType"] = "Alert";
                    
                    await deviceClient.SendEventAsync("alertOutput", previousCardsMessage);
                    Console.WriteLine("Completed sending alert");
                }

                await Task.FromResult(true);

                // Indicate that the message treatment is completed
                return MessageResponse.Completed;
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error in sample: {0}", exception);
                }

                // Indicate that the message treatment is not completed
                var deviceClient = (DeviceClient)userContext;
                return MessageResponse.Abandoned;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);

                // Indicate that the message treatment is not completed
                //DeviceClient deviceClient = (DeviceClient)userContext;
                return MessageResponse.Abandoned;
            }
        }

        private static async Task<bool> PersistDynoCard(DynoCard card)
        {
            try
            {
                // //Store the data in SQL db
                using (Sql.SqlConnection conn = new Sql.SqlConnection(_sqlConnectionString))
                {
                    conn.Open();
                    var insertHeader = new StringBuilder("INSERT INTO db4cards.[ACTIVE].[CARD_HEADER] ([EV_ID],[CH_COLLECTED],[CH_SCALED_MAX_LOAD],")
                    .Append("[CH_SCALED_MIN_LOAD],[CH_STROKE_LENGTH],[CH_STROKE_PERIOD],[CH_CARD_TYPE],[CH_UPDATE_DATE],[CH_UPDATE_BY]) ")
                    .Append("OUTPUT INSERTED.CH_ID ")
                    .Append($"VALUES (null, '{card.Timestamp}', {card.MaxLoad}, {card.MinLoad}, {card.StrokeLength}, {card.StrokePeriod}, {card.CardType}, '{DateTime.Now}', 'edgeModule');");
                    System.Console.WriteLine($"Header insert: {insertHeader}");

                    var insertDetail = "INSERT INTO db4cards.[ACTIVE].[CARD_DETAIL] ([CH_ID],[CD_POSITION],[CD_LOAD]) VALUES ({0}, {1}, {2});";

                    using (Sql.SqlCommand headerInsert = new Sql.SqlCommand(insertHeader.ToString(), conn))
                    {
                        var headerID = await headerInsert.ExecuteScalarAsync();

                        using (Sql.SqlCommand cmd = new Sql.SqlCommand())
                        {
                            cmd.Connection = conn;

                            foreach (var point in card.CardPoints)
                            {
                                string detailStatement = string.Format(insertDetail, headerID, point.Position, point.Load);
                                cmd.CommandText = detailStatement;
                                //System.Console.WriteLine($"Detail insert: {detailStatement}");
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error trying to insert dyno card data: {ex.Message}");
                System.Console.WriteLine(ex.StackTrace);
                await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private static async Task<List<DynoCard>> GetPreviousCards(DynoCardAnomaly anomalyCard)
        {
            DateTime start = anomalyCard.DynoCardTimestamp.Subtract(TimeSpan.FromHours(1));
            DateTime end = anomalyCard.DynoCardTimestamp;
            string startDateString = start.ToString("s");
            string endDateString = end.ToString("s");

            var sql = new StringBuilder("SELECT h.CH_ID, ")
            .Append("CH_COLLECTED, ")
            .Append("CH_SCALED_MAX_LOAD, ")
            .Append("CH_SCALED_MIN_LOAD, ")
            .Append("CH_STROKE_LENGTH, ")
            .Append("CH_STROKE_PERIOD, ")
            .Append("CH_CARD_TYPE, ")
            .Append("CD_ID, ")
            .Append("CD_POSITION, ")
            .Append("CD_LOAD ")
            .Append("FROM[ACTIVE].[CARD_HEADER] h ")
            .Append("JOIN[ACTIVE].[CARD_DETAIL] d ON h.CH_ID = d.CH_ID ")
            .Append($"WHERE h.CH_COLLECTED >= '{startDateString}' ")
            .Append($"AND h.CH_COLLECTED <= '{endDateString}' ")
            .Append("ORDER BY h.CH_ID DESC");

            Dictionary<int,DynoCard> cardList = new Dictionary<int,DynoCard>();
            
            // //Store the data in SQL db
            using (Sql.SqlConnection conn = new Sql.SqlConnection(_sqlConnectionString))
            {
                conn.Open();
            
                using (Sql.SqlCommand cardHistorySelect = new Sql.SqlCommand(sql.ToString(), conn))
                {
                    var results = await cardHistorySelect.ExecuteReaderAsync();

                    if (results.HasRows)
                    {
                        DynoCard card = null;
                        int headerID = 0;

                        while(await results.ReadAsync())
                        {
                            int currentID = results.GetInt32(0);

                            if (headerID != currentID)
                            {
                                headerID = currentID;

                                card = new DynoCard()
                                {
                                    Id = currentID,
                                    Timestamp = results.GetDateTime(1),
                                    MaxLoad = (int)results.GetFloat(2),
                                    MinLoad = (int)results.GetFloat(3),
                                    StrokeLength = (int)results.GetFloat(4),
                                    StrokePeriod = (int)results.GetFloat(5),
                                    CardType = results.GetString(6) == "0" ? DynoCardType.Pump : DynoCardType.Surface,
                                    CardPoints = new List<DynoCardPoint>()
                                };

                                card.CardPoints.Add(new DynoCardPoint()
                                {
                                    Position = (int)results.GetFloat(8),
                                    Load = (int)results.GetFloat(9)
                                });

                                cardList.Add(currentID, card);
                            }
                            else
                            {
                                cardList[currentID].CardPoints.Add(new DynoCardPoint()
                                {
                                    Position = (int)results.GetFloat(8),
                                    Load = (int)results.GetFloat(9)
                                });
                            }
                        }
                    }
                }
            }

            return await Task.FromResult(cardList?.Values?.ToList());
        }
    }
}
