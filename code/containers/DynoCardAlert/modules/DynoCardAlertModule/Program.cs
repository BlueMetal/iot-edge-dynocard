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
using DynoCardAlertModule.Model;
using DynoCardAlertModule.Data;
using DynoCardAlertModule.Config;

namespace DynoCardAlertModule
{    class Program
    {
        private static int counter;

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
            ModbusMessage.SurfaceLayoutConfig = modbusConfig.SurfaceCardConfiguration;
            ModbusMessage.PumpLayoutConfig = modbusConfig.PumpCardConfiguration;

            string sqlConnectionString = desiredProperties["sqlConnectionString"];
            DataHelper.ConnectionString = sqlConnectionString;
            Console.WriteLine($"sqlConnectionString: {sqlConnectionString}");

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
                ModbusMessage.SurfaceLayoutConfig = modbusConfig.SurfaceCardConfiguration;
                ModbusMessage.PumpLayoutConfig = modbusConfig.PumpCardConfiguration;

                string sqlConnectionString = desiredProperties["sqlConnectionString"];
                DataHelper.ConnectionString = sqlConnectionString;
                Console.WriteLine($"sqlConnectionString: {sqlConnectionString}");
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
        
        private static async Task<MessageResponse> WriteModusValue(string message, DeviceClient client)
        {
            //Write the message back to the modbus interface
            Console.WriteLine($"Processing modbus write value: {message}");

            var bytes = Encoding.UTF8.GetBytes(message);
            Message modbusWriteMessage = new Message(bytes);
            modbusWriteMessage.Properties.Add("command-type", "ModbusWrite");
            
            await client.SendEventAsync("modbusWriteOutput", message);
            Console.WriteLine("Completed modbus write value");

            return MessageResponse.Completed;
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
                List<DynoCard> cards = new List<DynoCard>();

                //Both surface and pump card values are in a single reading. Both are used to create a single dyno card.
                if (modbusMessage != null )
                {
                    var dynoCard = await modbusMessage.ToDynoCard();
                    
                    if (dynoCard.SurfaceCard != null && dynoCard.PumpCard != null)
                    {
                        cards.Add(dynoCard);
                        Console.WriteLine("Parsing dyno card values.");
                    }
                }

                foreach (var card in cards)
                {
                    string json = JsonConvert.SerializeObject(card);
                    //System.Console.WriteLine(json);

                    int cardID = await DataHelper.PersistDynoCard(card);
                    card.Id = cardID;

                    var dynoCardMessage = card.ToDeviceMessage();
                    await deviceClient.SendEventAsync("output1", dynoCardMessage);
                }

                //Reset regsiter value to 0
                string writeMessage0 = "{\"HwId\": \"Pump1-DynoCard\", \"UId\":\"1\", \"Address\":\"00109\", \"Value\":\"0\"}";
                await WriteModusValue(writeMessage0, deviceClient);

                //Reset register value to 1 to refresh buffer
                string writeMessage1 = "{\"HwId\": \"Pump1-DynoCard\", \"UId\":\"1\", \"Address\":\"00109\", \"Value\":\"1\"}";
                await WriteModusValue(writeMessage1, deviceClient);

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
                return MessageResponse.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
                // Indicate that the message treatment is not completed
                //DeviceClient deviceClient = (DeviceClient)userContext;
                return MessageResponse.None;
            }
        }

        private static async Task<MessageResponse> ProcessOPCInput(Message message, object userContext)
        {
            Console.WriteLine("In the OPC Filter Message handler");

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
                Console.WriteLine($"Received anomaly notification {counterValue}: [{messageString}]");

                if (!string.IsNullOrEmpty(messageString))
                {
                    var alertMessage = JsonConvert.DeserializeObject<DynoCardAnomalyResult>(messageString);
                    string anomalyString = alertMessage.Anomaly;
                    bool isAnomaly = false;

                    if (!(string.IsNullOrEmpty(anomalyString)))
                    {
                        isAnomaly = Boolean.Parse(anomalyString);
                    }

                    if (isAnomaly)
                    {
                        var previousCardList = await DataHelper.GetPreviousCards(alertMessage);

                        if (previousCardList != null && previousCardList.Count > 0)
                        {
                            Console.WriteLine($"Number of previous entries: {previousCardList.Count}");

                            var previousCardBytes = CreateByteArray(previousCardList); 
                            Console.WriteLine($"Number bytes in the array: {previousCardBytes.Length}");

                            //Device-to-Cloud message size limit is 256KB, so we need to limit it if the message is larger than that
                            while (previousCardBytes.Length > 256 * 1000)
                            {
                                var card = previousCardList[0];
                                Console.WriteLine($"Removing card #: {card.Id}, to reduce message size");

                                previousCardList.RemoveAt(0);
                                previousCardBytes = CreateByteArray(previousCardList);
                            }

                            DynoCardAnomalyEvent dynoCardAnomaly = new DynoCardAnomalyEvent()
                            {
                                Id = Int32.Parse(alertMessage.Id),
                                Timestamp = alertMessage.Timestamp,
                                DynoCards = previousCardList,
                                Pump = 1
                            };

                            var dynoCardAnomalyString = JsonConvert.SerializeObject(dynoCardAnomaly);
                            var dynoCardAnomalyBytes = Encoding.UTF8.GetBytes(dynoCardAnomalyString);
                            var previousCardsMessage = new Message(dynoCardAnomalyBytes);

                            Console.WriteLine("Sending Alert");
                            previousCardsMessage.Properties["MessageType"] = "Alert";
                            await deviceClient.SendEventAsync("alertOutput", previousCardsMessage);

                            var stopModbusString = JsonConvert.SerializeObject(new StopModbusMessage());
                            var stopModbusBytes = Encoding.UTF8.GetBytes(stopModbusString);
                            var stopModbusMessage = new Message(stopModbusBytes);
                            await deviceClient.SendEventAsync("shutdownOutput", stopModbusMessage);

                            Console.WriteLine("Completed sending alert");
                        }
                    }
                }

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
                return MessageResponse.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);

                // Indicate that the message treatment is not completed
                //DeviceClient deviceClient = (DeviceClient)userContext;
                return MessageResponse.None;
            }
        }

        private static byte[] CreateByteArray(List<DynoCard> cardList)
        {
            var cardListString = JsonConvert.SerializeObject(cardList);
            var cardListBytes = Encoding.UTF8.GetBytes(cardListString);

            return cardListBytes;
        }
    }
}
