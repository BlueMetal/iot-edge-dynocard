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
        private static Timer _timer;

        private const int NUMBER_OF_SURFACE_POINTS = 200;
        private const int NUMBER_OF_PUMP_POINTS = 100;

        //Default Pump and Surface Card values
        private static int[] surfaceLoadArray = { 11744, 11259, 10801, 10667, 10804, 10764, 10892, 11126, 11401, 11518, 11576, 11713, 11910, 11992, 12031, 12082, 12130, 12146, 12132, 12223, 12279, 12232, 12169, 12076, 11790, 11379, 11088, 10719, 10368, 10016, 9631, 9424, 9237, 9072, 8931, 8840, 8722, 8590, 8457, 8433, 8411, 8463, 8657, 8812, 9127, 9511, 9867, 10305, 10656, 10878, 10928, 10896, 10804, 10718, 10705, 10647, 10552, 10469, 10453, 10121, 9890, 9672, 9319, 8831, 8377, 7970, 7613, 7382, 7376, 7592, 7879, 8156, 8443, 8734, 8907, 9155, 9395, 9588, 9852, 10211, 10556, 10826, 11122, 11468, 11848, 12227, 12387, 12538, 12775, 13073, 13177, 13320, 13423, 13492, 13581, 13709, 13882, 14077, 14262, 14271, 14348, 14501, 14693, 14674, 14902, 15352, 15854, 15996, 15819, 15682, 15544, 15319, 14984, 14658, 14466, 14406, 14090, 14042, 14031, 13973, 13817, 13586, 13556, 13671, 13840, 14052, 14286, 14482, 14666, 15235, 15564, 15774, 15937, 16170, 16376, 16458, 16461, 16643, 16740, 16857, 16943, 16721, 16541, 16416, 16309, 16066, 15702, 15401, 15173, 14858, 14570, 14513, 14688, 14928, 15099, 15267, 15435, 15544, 15788, 16127, 16511, 16694, 17070, 17531, 18030, 18565, 18999, 19384, 19802, 19594, 19231, 18816, 18450, 18188, 17918, 17570, 17306, 17060, 16792, 16453, 16079, 15729, 15406, 15109, 14637, 14136, 13690, 13440, 13211, 13076, 13022, 13010, 12866, 12821, 12812, 12707, 12525, 12356, 12243, 12229 };
        private static int[] surfacePositionArray = { 1, 16, 33, 50, 63, 114, 165, 216, 268, 346, 430, 515, 600, 708, 825, 942, 1058, 1193, 1338, 1483, 1628, 1786, 1953, 2120, 2288, 2464, 2646, 2828, 3009, 3195, 3386, 3576, 3766, 3958, 4151, 4344, 4537, 4730, 4921, 5113, 5304, 5495, 5682, 5869, 6055, 6244, 6424, 6604, 6784, 6965, 7138, 7308, 7477, 7646, 7810, 7968, 8126, 8283, 8437, 8582, 8727, 8872, 9015, 9147, 9279, 9410, 9541, 9662, 9780, 9900, 10019, 10131, 10238, 10345, 10453, 10555, 10650, 10744, 10840, 10932, 11013, 11095, 11178, 11259, 11327, 11395, 11464, 11533, 11586, 11640, 11694, 11747, 11787, 11825, 11863, 11904, 11925, 11945, 11966, 11986, 11993, 11995, 11996, 11998, 11986, 11968, 11949, 11930, 11899, 11858, 11818, 11778, 11725, 11662, 11599, 11536, 11462, 11376, 11290, 11204, 11108, 11000, 10892, 10783, 10667, 10537, 10409, 10280, 10147, 10000, 9853, 9707, 9558, 9396, 9235, 9073, 8912, 8738, 8565, 8392, 8219, 8038, 7856, 7674, 7491, 7304, 7115, 6925, 6735, 6544, 6351, 6159, 5967, 5775, 5584, 5393, 5202, 5013, 4827, 4641, 4456, 4271, 4094, 3917, 3741, 3566, 3396, 3231, 3065, 2899, 2736, 2582, 2427, 2271, 2116, 1975, 1835, 1694, 1548, 1426, 1303, 1180, 1056, 946, 844, 743, 641, 550, 473, 397, 321, 252, 204, 157, 109, 65, 49, 32, 16, 8 };
        private static int[] pumpLoadArray = { 1, 585, 1169, 1753, 2337, 2921, 3505, 4089, 4673, 5257, 5841, 6288, 6380, 6402, 6394, 6406, 6413, 6419, 6426, 6433, 6440, 6447, 6454, 6460, 6467, 6474, 6481, 6488, 6495, 6501, 6508, 6515, 6522, 6529, 6536, 6542, 6549, 6556, 6563, 6645, 6652, 6659, 6665, 6672, 6679, 6686, 6693, 6700, 6706, 6713, 6720, 6727, 6734, 6250, 5766, 5282, 4799, 4315, 3831, 3347, 2864, 2380, 1896, 1412, 928, 445, 425, 405, 386, 366, 346, 327, 307, 287, 268, 248, 228, 209, 189, 169, 150, 140, 130, 120, 110, 95, 75, 65, 55, 45, 40, 35, 30, 25, 20, 15, 10, 5, 3, 3 };
        private static int[] pumpPositionArray = { 1, 20, 40, 59, 79, 98, 118, 137, 157, 176, 196, 350, 500, 950, 1147, 1407, 1667, 1927, 2187, 2447, 2707, 2967, 3227, 3488, 3748, 4008, 4268, 4528, 4788, 5048, 5308, 5568, 5828, 6088, 6348, 6609, 6869, 7129, 7389, 7649, 7909, 8169, 8429, 8689, 8949, 9209, 9469, 9730, 9990, 10250, 10510, 10770, 10950, 11000, 10970, 10940, 10910, 10880, 10850, 10820, 10790, 10760, 10731, 10701, 10671, 10641, 10500, 10200, 10050, 9800, 9350, 8979, 8650, 8200, 7846, 7500, 7020, 6550, 6000, 5641, 5199, 4650, 4050, 3500, 3250, 3000, 2750, 2500, 2250, 2000, 1750, 1500, 1200, 950, 750, 550, 400, 300, 150, 50 };

        //Gas Interference anomaly Pump and Surface Card values
        private static int[] gasInterferenceSurfaceLoadArray = { 13954, 14161, 14385, 14498, 14317, 14266, 14473, 14697, 14939, 15043, 15353, 15561, 15664, 16001, 16002, 15976, 15951, 15951, 15977, 16004, 16082, 16134, 16134, 16134, 16394, 16575, 16773, 16989, 17187, 17714, 17921, 18309, 18533, 19060, 19189, 19319, 19397, 19475, 19760, 19760, 19709, 19709, 19709, 19477, 19323, 19194, 19013, 18910, 18600, 18497, 18394, 18291, 18162, 17723, 17646, 17646, 17569, 17234, 17027, 16803, 16459, 16614, 15891, 15684, 15503, 15400, 15349, 15065, 14885, 14859, 14833, 14782, 14654, 14680, 14783, 14887, 15068, 15457, 15561, 15794, 15924, 16157, 16157, 16054, 16029, 16055, 16288, 16314, 16470, 16056, 15850, 15669, 15229, 15022, 14815, 14608, 14401, 14298, 14168, 14194, 14012, 13986, 14089, 14089, 14011, 13855, 13725, 13699, 13621, 13621, 13647, 13775, 13801, 13826, 13826, 13826, 13695, 13514, 13358, 13203, 12788, 12556, 12340, 12141, 11960, 11391, 11236, 11098, 10873, 10718, 10304, 10097, 9855, 9683, 9476, 9234, 9061, 9027, 8854, 8854, 8491, 8439, 8335, 8283, 8180, 8075, 8075, 8075, 8075, 7970, 7944, 7892, 7866, 7840, 7503, 7347, 7347, 7347, 7346, 7552, 7655, 7810, 7991, 8404, 8585, 8843, 8740, 9033, 9670, 9876, 10066, 10264, 10445, 10987, 11168, 11271, 11633, 12253, 12098, 12460, 12304, 12666, 12976, 13131, 13234, 13389, 13595, 13801, 13853, 13904, 13852, 13231, 13050, 12817, 12610, 12714, 13334, 13153, 13747, 13541 };
        private static int[] gasInterferenceSurfacePositionArray = { 1, 27, 62, 155, 257, 511, 563, 622, 716, 818, 1023, 1125, 1227, 1431, 1737, 1839, 1940, 2042, 2144, 2449, 2551, 2653, 2755, 2857, 3163, 3265, 3325, 3419, 3479, 3624, 3676, 3778, 3838, 4136, 4238, 4341, 4443, 4545, 4851, 4952, 5054, 5156, 5258, 5563, 5664, 5765, 5867, 5968, 6273, 6375, 6476, 6578, 6679, 6983, 7085, 7289, 7390, 7695, 7796, 7855, 7948, 7948, 8201, 8302, 8403, 8505, 8607, 8911, 9013, 9114, 9216, 9318, 9623, 9725, 9827, 9929, 10031, 10337, 10439, 10644, 10746, 10950, 11052, 11255, 11357, 11459, 11663, 11765, 11943, 11968, 11967, 11967, 11940, 11914, 11914, 11939, 11938, 11836, 11759, 11658, 11454, 11352, 11148, 11047, 10945, 10842, 10537, 10435, 10333, 10231, 10129, 9824, 9723, 9621, 9519, 9417, 9112, 8908, 8805, 8703, 8397, 8345, 8252, 8192, 8166, 7986, 7960, 7892, 7832, 7807, 7627, 7525, 7431, 7320, 7295, 7124, 7090, 7022, 6988, 6911, 6605, 6503, 6401, 6299, 6197, 5891, 5790, 5688, 5586, 5179, 5077, 4975, 4873, 4771, 4465, 4363, 4261, 4159, 4057, 3651, 3549, 3448, 3346, 3195, 3144, 3094, 3068, 3001, 2791, 2689, 2596, 2537, 2487, 2336, 2311, 2234, 2134, 1881, 1880, 1779, 1779, 1678, 1475, 1374, 1272, 1171, 1069, 764, 663, 459, 357, 254, 254, 228, 227, 160, 51, 50, 1, 0 };
        private static int[] gasInterferencePumpLoadArray = { 1490, 1940, 2615, 1884, 2560, 2560, 2588, 3010, 3685, 4136, 4811, 5261, 6049, 6518, 6669, 6585, 6474, 6531, 6448, 6505, 6507, 6311, 6396, 6285, 6286, 6399, 6401, 6345, 6234, 6319, 6377, 6294, 6239, 6296, 6241, 6326, 6355, 6412, 6385, 6415, 6472, 6529, 6531, 6438, 6644, 6617, 6703, 6648, 6705, 6621, 6454, 6089, 5864, 5751, 5553, 5440, 5242, 4866, 4415, 3946, 3270, 3045, 2257, 1694, 1132, 906, 399, 455, 594, 593, 508, 563, 618, 505, 503, 502, 445, 444, 499, 582, 553, 552, 495, 437, 492, 379, 378, 433, 376, 375, 289, 176, 146, 258, 3, 143, 620, 788, 422, 816 };
        private static int[] gasInterferencePumpPositionArray = { 1, 1, 1, 73, 110, 212, 323, 323, 379, 406, 462, 490, 601, 721, 1045, 1267, 1601, 1823, 2156, 2379, 2712, 2934, 3267, 3490, 3823, 4045, 4378, 4601, 4934, 5156, 5490, 5712, 6045, 6267, 6601, 6823, 6934, 7267, 7490, 7934, 8267, 8489, 8823, 9054, 9156, 9378, 9712, 9934, 10156, 10378, 10712, 10934, 10962, 10767, 10545, 10323, 9989, 9776, 9554, 9434, 9267, 9212, 9110, 8989, 8888, 8851, 8628, 8434, 8101, 7878, 7545, 7323, 6990, 6767, 6434, 6212, 5878, 5656, 5323, 5101, 4767, 4545, 4212, 3990, 3656, 3434, 3101, 2879, 2545, 2323, 1990, 1767, 1434, 1212, 879, 656, 434, 212, 1, 1 };

        private static int surfaceScaledMinLoad;
        private static int surfaceScaledMaxLoad;
        private static int surfaceStrokeLength;
        private static int surfaceStrokePeriod;

        private static int pumpScaledMinLoad;
        private static int pumpScaledMaxLoad;
        private static int pumpNetStrokeLength;
        private static int pumpGrossStrokeLength;
        private static int pumpFluidLoad;
        private static int pumpFillage;

        private static Dictionary<AnomalyType, StaticDataContainer> cardDataList;

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

                cardDataList = new Dictionary<AnomalyType, StaticDataContainer>();
                cardDataList.Add(AnomalyType.None, new StaticDataContainer()
                {
                    SurfaceLoadValues = surfaceLoadArray,
                    SurfacePositionValues = surfacePositionArray,
                    PumpLoadValues = pumpLoadArray,
                    PumpPositionValues = pumpPositionArray
                });

                cardDataList.Add(AnomalyType.GasInterference, new StaticDataContainer()
                {
                    SurfaceLoadValues = gasInterferenceSurfaceLoadArray,
                    SurfacePositionValues = gasInterferenceSurfacePositionArray,
                    PumpLoadValues = gasInterferencePumpLoadArray,
                    PumpPositionValues = gasInterferencePumpPositionArray
                });

                Console.WriteLine("Setting module twin property handler");
                // Attach callback for Twin desired properties updates
                await ioTHubModuleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, null);

                //Start a timer to send data to the IoT Hub every minute
                _timer = new Timer(GenerateTelemetry, ioTHubModuleClient, TimeSpan.Zero, TimeSpan.FromSeconds(5));
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
                _timer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(frequencyInSeconds));
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

        private static async void GenerateTelemetry(object state)
        {
            try
            {
                System.Console.WriteLine("Running the telemetry generator.");

                var moduleClient = (ModuleClient)state;

                var staticData = cardDataList[AnomalyType.GasInterference];
                string dynocardTelemetryMessage = BuildDynoCardMessage(staticData);
                var dynocardTelemetryBytes = Encoding.UTF8.GetBytes(dynocardTelemetryMessage);
                var telemetryMessage = new Message(dynocardTelemetryBytes);

                System.Console.WriteLine($"Sending telemetry: {dynocardTelemetryMessage}");
                await moduleClient.SendEventAsync("telemetryOutput", telemetryMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error when processing telemetry generator: {0}", ex.Message);
            }

            await Task.FromResult(true);
            System.Console.WriteLine("Finished generating telemetry.");
        }

        private static string BuildDynoCardMessage(StaticDataContainer data)
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
