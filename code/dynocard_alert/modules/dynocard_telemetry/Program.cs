using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telemetry.Model;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

namespace dynocard_telemetry
{
    class Program
    {
        private static int counter;
        private static Timer timer;
        private static AnomalyType CurrentAnomalyType = AnomalyType.None;

        private const int NUMBER_OF_SURFACE_POINTS = 200;
        private const int NUMBER_OF_PUMP_POINTS = 100;

        //Surface card properties
        private static int surfaceScaledMinLoad;
        private static int surfaceScaledMaxLoad;
        private static int surfaceStrokeLength;
        private static int surfaceStrokePeriod;

        //Pump card properties
        private static int pumpScaledMinLoad;
        private static int pumpScaledMaxLoad;
        private static int pumpNetStrokeLength;
        private static int pumpGrossStrokeLength;
        private static int pumpFluidLoad;
        private static int pumpFillage;

        private static Dictionary<AnomalyType, IDynocardDataSnapshot> cardDataList;

        static void Main(string[] args)
        {
            Init().Wait();

            // Wait until the app unloads or is cancelled
            var cts = new CancellationTokenSource();
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        /// <summary>
        /// Initializes the ModuleClient and sets up the callback to receive
        /// messages containing temperature information
        /// </summary>
        static async Task Init()
        {
            try
            {
                AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
                ITransportSettings[] settings = { amqpSetting };

                // Open a connection to the Edge runtime
                ModuleClient ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
                await ioTHubModuleClient.OpenAsync();
                Console.WriteLine("IoT Hub module client initialized.");

                surfaceScaledMinLoad = Int32.Parse(ConfigurationManager.AppSettings["SurfaceCard.ScaledMinLoad"]);
                surfaceScaledMaxLoad = Int32.Parse(ConfigurationManager.AppSettings["SurfaceCard.ScaledMaxLoad"]);
                surfaceStrokeLength = Int32.Parse(ConfigurationManager.AppSettings["SurfaceCard.StrokeLength"]);
                surfaceStrokePeriod = Int32.Parse(ConfigurationManager.AppSettings["SurfaceCard.StrokePeriod"]);

                pumpScaledMinLoad = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.ScaledMinLoad"]);
                pumpScaledMaxLoad = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.ScaledMaxLoad"]);
                pumpNetStrokeLength = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.NetStrokeLength"]);
                pumpGrossStrokeLength = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.GrossStrokeLength"]);
                pumpFillage = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.PumpFillage"]);
                pumpFluidLoad = Int32.Parse(ConfigurationManager.AppSettings["PumpCard.FluidLoad"]);

                cardDataList = new Dictionary<AnomalyType, IDynocardDataSnapshot>();
                cardDataList.Add(AnomalyType.None, new DefaultSnapshot());
                cardDataList.Add(AnomalyType.GasInterference, new GasInterferenceSnapshot());

                // Attach callback for Twin desired properties updates
                Console.WriteLine("Setting module twin property handler");
                await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);

                //Attach callback for anomaly change event handler
                Console.WriteLine("Setting handler for anomaly change event");
                await ioTHubModuleClient.SetInputMessageHandlerAsync("anomalyChange", AnomalyChangeEvent, ioTHubModuleClient);

                //Start a timer to send data to the IoT Hub every minute
                timer = new Timer(GenerateTelemetry, ioTHubModuleClient, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error running Init method: {ex.Message}");
                System.Console.WriteLine(ex.StackTrace);
            }
        }

        private static Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                string json = JsonConvert.SerializeObject(desiredProperties);
                Console.WriteLine("Desired property change:");

                ModuleClient moduleClient = (ModuleClient)userContext;
                int frequencyInSeconds = desiredProperties["telemetryFrequencyInSecs"];
                Console.WriteLine($"Timer frequency in seconds: {frequencyInSeconds}");
                
                System.Console.WriteLine($"Starting a new timer for every {frequencyInSeconds} seconds.");
                timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(frequencyInSeconds));
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

        private static async Task<MessageResponse> AnomalyChangeEvent(Message message, object userContext)
        {
            try
            {
                Console.WriteLine("In the AnomalyChangeEvent Message handler");
                ModuleClient moduleClient = (ModuleClient)userContext;

                await Task.Run(() => 
                {
                    var messageBytes = message.GetBytes();
                    var messageString = Encoding.UTF8.GetString(messageBytes);
                    
                    if (Enum.IsDefined(typeof(AnomalyType), messageString))
                    {
                        var updatedAnomaly = (AnomalyType)Enum.Parse(typeof(AnomalyType), messageString);
                        CurrentAnomalyType = updatedAnomaly;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when processing AnomalyChangeEvent message: {0}", ex.Message);
            }

            return MessageResponse.Completed;
        }

        private static async void GenerateTelemetry(object state)
        {
            try
            {
                System.Console.WriteLine("Running the telemetry generator.");

                var moduleClient = (ModuleClient)state;

                var staticData = cardDataList[CurrentAnomalyType];
                string dynocardTelemetryMessage = BuildDynoCardMessage(staticData);
                var dynocardTelemetryBytes = Encoding.UTF8.GetBytes(dynocardTelemetryMessage);
                var telemetryMessage = new Message(dynocardTelemetryBytes);

                System.Console.WriteLine($"Sending telemetry"); // {dynocardTelemetryMessage}");
                await moduleClient.SendEventAsync("telemetryOutput", telemetryMessage);

                var anomalyChangeMessageBytes = Encoding.UTF8.GetBytes("GasInterference");
                var anomalyChangeMessage = new Message(anomalyChangeMessageBytes);
                await moduleClient.SendEventAsync("anomalyChangeTemp", anomalyChangeMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when processing telemetry generator: {0}", ex.Message);
            }

            await Task.FromResult(true);
            System.Console.WriteLine("Finished generating telemetry.");
        }

        private static string BuildDynoCardMessage(IDynocardDataSnapshot data)
        {
            var timestamp = DateTime.Now;
            var random = new Random();
            List<CardCoordinate> surfacePoints = new List<CardCoordinate>();
            List<CardCoordinate> pumpPoints = new List<CardCoordinate>();

            //Load the 200 surface points
            for(int i = 0; i < NUMBER_OF_SURFACE_POINTS; i++)
            {
                surfacePoints.Add(new CardCoordinate()
                {
                    Order = i,
                    Load = random.Next((int)(data.SurfaceLoadValues[i] * .97), (int)(data.SurfaceLoadValues[i] * 1.02)),
                    Position = data.SurfacePositionValues[i] //random.Next((int)(surfacePositionArray[i] * .97), (int)(surfacePositionArray[i] * 1.02))
                });
            }

            //Load the 100 pump points
            for (int i = 0; i < NUMBER_OF_PUMP_POINTS; i++)
            {
                pumpPoints.Add(new CardCoordinate()
                {
                    Order = i,
                    Load = random.Next((int)(data.PumpLoadValues[i] * .97), (int)(data.PumpLoadValues[i] * 1.02)),
                    Position = data.PumpPositionValues[i] //random.Next((int)(pumpPositionArray[i] * .97), (int)(pumpPositionArray[i] * 1.02))
                });
            }

            DynoCard card =  new DynoCard
            {
                Timestamp = timestamp,
                PumpCard = new PumpCard()
                {
                    Timestamp = timestamp,
                    NumberOfPoints = NUMBER_OF_PUMP_POINTS * 2,
                    ScaledMinLoad = pumpScaledMinLoad,
                    ScaledMaxLoad = pumpScaledMaxLoad,
                    NetStroke = pumpNetStrokeLength,
                    GrossStroke = pumpGrossStrokeLength,
                    FluidLoad = pumpFluidLoad,
                    PumpFillage = pumpFillage,
                    CardType = CardType.Pump,
                    CardCoordinates = pumpPoints
                },
                SurfaceCard = new SurfaceCard()
                {
                    Timestamp = timestamp,
                    NumberOfPoints = NUMBER_OF_SURFACE_POINTS * 2,
                    ScaledMinLoad = surfaceScaledMinLoad,
                    ScaledMaxLoad = surfaceScaledMaxLoad,
                    StrokeLength = surfaceStrokeLength,
                    StrokePeriod = surfaceStrokePeriod,
                    CardType = CardType.Surface,
                    CardCoordinates = surfacePoints
                }
            };

            counter++;
            return JsonConvert.SerializeObject(card);
        }
    }
}
