using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynoCardAlertModule.Config;
using DynoCardAlertModule.Data;
using System.Threading.Tasks;

namespace DynoCardAlertModule.Model
{
     public class ModbusMessage
    {
        public string DisplayName { get; set; }
        public List<ModbusRegisterValue> SurfaceCardRegisterValues { get; set; }
        public List<ModbusRegisterValue> PumpCardRegisterValues { get; set; }
        public static SurfaceCardConfig SurfaceLayoutConfig { get; set; }
        public static PumpCardConfig PumpLayoutConfig { get; set; }

        public ModbusMessage(Message message)
        {
            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);
            
            if (!string.IsNullOrEmpty(messageString))
            {
                var registers = JsonConvert.DeserializeObject<List<ModbusRegisterValue>>(messageString);

                if (registers != null && registers.Count > 0)
                {
                    //Sort the list so we know what the first Op name is
                    registers = registers.OrderBy(r => r.Address).ToList();
               
                    SurfaceCardRegisterValues = FilterSurfaceCardRegisters(registers);
                    PumpCardRegisterValues = FilterPumpCardRegisters(registers);
                    
                    Console.WriteLine("Completed creating modbus message");
                }
                else
                {
                    Console.WriteLine("Empty list of register values");
                }
            }
            else
            {
                Console.WriteLine("Empty modbus message received");
            }
        }

        private static List<ModbusRegisterValue> FilterSurfaceCardRegisters(List<ModbusRegisterValue> fullList)
        {
            var surfaceCardRegisters = fullList.Where(c => c.Address >= SurfaceLayoutConfig.Timestamp.RegisterNumber && c.Address < PumpLayoutConfig.Timestamp.RegisterNumber);
            var sortedList = surfaceCardRegisters.OrderBy(r => r.Address).ToList();
            return sortedList;
        }

        private static List<ModbusRegisterValue> FilterPumpCardRegisters(List<ModbusRegisterValue> fullList)
        {
            var pumpCardRegisters = fullList.Where(c => c.Address >= PumpLayoutConfig.Timestamp.RegisterNumber);
            var sortedList = pumpCardRegisters.OrderBy(r => r.Address).ToList();
            return sortedList;
        }
    }

    public static class ModbusExtensions
    {
        public static Message ToDeviceMessage(this DynoCard dynoCard)
        {
            Message dynoCardMessage = null;

            if (dynoCard != null)
            {
                var dynoCardBytes = JsonConvert.SerializeObject(dynoCard);
                var dynoCardByteString = Encoding.UTF8.GetBytes(dynoCardBytes);
                dynoCardMessage = new Message(dynoCardByteString);
            }

            return dynoCardMessage;
        }

        public static async Task<DynoCard> ToDynoCard(this ModbusMessage modbus)
        {
            DynoCard dynoCard = new DynoCard()
            {
                SurfaceCard = PopulateSurfaceCard(modbus.SurfaceCardRegisterValues),
                PumpCard = PopulatePumpCard(modbus.PumpCardRegisterValues),
                TriggeredEvents = false
            };

            dynoCard.Timestamp = dynoCard.SurfaceCard.Timestamp;
            return await Task.FromResult(dynoCard);      
        }

        private static SurfaceCard PopulateSurfaceCard(List<ModbusRegisterValue> registerValues)
        {
            if (registerValues == null || registerValues.Count == 0)
            {
                System.Console.WriteLine("Empty register values passed.");
                return null;
            }

            System.Console.WriteLine("Parsing surface card.");
            SurfaceCard surfaceCard = new SurfaceCard();

            var timestampProp = ModbusMessage.SurfaceLayoutConfig.Timestamp;
            if (timestampProp != null)
            {
                int timestamp = 0;
                var timeStampValues = GetValueArray(timestampProp.RegisterNumber, timestampProp.NumberOfRegisters, registerValues);

                if (timeStampValues != null && timeStampValues.Count > 1)
                {
                    short left = (short)Int32.Parse(timeStampValues[0]);
                    short right = (short)Int32.Parse(timeStampValues[1]);

                    timestamp = left;
                    timestamp = (timestamp << 16);
                    timestamp = timestamp | (int)(ushort)right;
                }

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
                surfaceCard.Timestamp = dateTime;
            }

            var maxLoadProp = ModbusMessage.SurfaceLayoutConfig.MaxLoad;
            if (maxLoadProp != null)
            {
                var maxLoadpValues = GetValueArray(maxLoadProp.RegisterNumber, maxLoadProp.NumberOfRegisters, registerValues);
                if (maxLoadpValues != null && maxLoadpValues.Count > 0)
                {
                    surfaceCard.ScaledMaxLoad = Int32.Parse(maxLoadpValues.First());
                }
            }

            var minLoadProp = ModbusMessage.SurfaceLayoutConfig.MinLoad;
            if (minLoadProp != null)
            {
                var minLoadValues = GetValueArray(minLoadProp.RegisterNumber, minLoadProp.NumberOfRegisters, registerValues);
                if (minLoadValues != null && minLoadValues.Count > 0)
                {
                    surfaceCard.ScaledMinLoad = Int32.Parse(minLoadValues.First());
                }
            }

            var strokeLengthProp = ModbusMessage.SurfaceLayoutConfig.StrokeLength;
            if (strokeLengthProp != null)
            {
                var strokeLengthValues = GetValueArray(strokeLengthProp.RegisterNumber, strokeLengthProp.NumberOfRegisters, registerValues);
                if (strokeLengthValues != null && strokeLengthValues.Count > 0)
                {
                    surfaceCard.StrokeLength = Int32.Parse(strokeLengthValues.First());
                }
            }

            var strokePeriodProp = ModbusMessage.SurfaceLayoutConfig.StrokePeriod;
            if (strokePeriodProp != null)
            {
                var strokePeriodValues = GetValueArray(strokePeriodProp.RegisterNumber, strokePeriodProp.NumberOfRegisters, registerValues);
                if (strokePeriodValues != null && strokePeriodValues.Count > 0)
                {
                    surfaceCard.StrokePeriod = Int32.Parse(strokePeriodValues.First());
                }
            }

            var numberOfPointsProp = ModbusMessage.SurfaceLayoutConfig.NumberOfPoints;
            int numberOfDataPoints = 0;
            if (numberOfPointsProp != null)
            {
                var numberOfPointsValues = GetValueArray(numberOfPointsProp.RegisterNumber, numberOfPointsProp.NumberOfRegisters, registerValues);
                if (numberOfPointsValues != null && numberOfPointsValues.Count > 0)
                {
                    numberOfDataPoints = Int32.Parse(numberOfPointsValues.First());
                    surfaceCard.NumberOfPoints = numberOfDataPoints;
                }

                List<CardCoordinate> cardCoordinates = new List<CardCoordinate>();
                var pointArrayProperty = ModbusMessage.SurfaceLayoutConfig.Point;

                for (int i = 0; i < surfaceCard.NumberOfPoints; i += numberOfPointsProp.NumberOfRegisters)
                {
                    var pointsArray = GetValueArray(pointArrayProperty.RegisterNumber + i, pointArrayProperty.NumberOfRegisters, registerValues);
                    if (pointsArray != null && pointsArray.Count > 1)
                    {
                        cardCoordinates.Add(new CardCoordinate()
                        {
                            Load = Int32.Parse(pointsArray[0]),
                            Position = Int32.Parse(pointsArray[1])
                        });
                    }
                }

                surfaceCard.CardCoordinates = cardCoordinates;
            }

            return surfaceCard;
        }

        private static PumpCard PopulatePumpCard(List<ModbusRegisterValue> registerValues)
        {
            if (registerValues == null || registerValues.Count == 0)
            {
                System.Console.WriteLine("Empty register values passed.");
                return null;
            }

            PumpCard pumpCard = new PumpCard();
            Console.WriteLine("Parsing pump card.");

            var timestampProp = ModbusMessage.PumpLayoutConfig.Timestamp;
            if (timestampProp != null)
            {
                int timestamp = 0;
                var timeStampValues = GetValueArray(timestampProp.RegisterNumber, timestampProp.NumberOfRegisters, registerValues);

                if (timeStampValues != null && timeStampValues.Count > 1)
                {
                    short left = (short)Int32.Parse(timeStampValues[0]);
                    short right = (short)Int32.Parse(timeStampValues[1]);

                    timestamp = left;
                    timestamp = (timestamp << 16);
                    timestamp = timestamp | (int)(ushort)right;
                }

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
                pumpCard.Timestamp = dateTime;
            }

            var maxLoadProp = ModbusMessage.PumpLayoutConfig.MaxLoad;
            if (maxLoadProp != null)
            {
                var maxLoadpValues = GetValueArray(maxLoadProp.RegisterNumber, maxLoadProp.NumberOfRegisters, registerValues);
                if (maxLoadpValues != null && maxLoadpValues.Count > 0)
                {
                    pumpCard.ScaledMaxLoad = Int32.Parse(maxLoadpValues.First());
                }
            }

            var minLoadProp = ModbusMessage.PumpLayoutConfig.MinLoad;
            if (minLoadProp != null)
            {
                var minLoadValues = GetValueArray(minLoadProp.RegisterNumber, minLoadProp.NumberOfRegisters, registerValues);
                if (minLoadValues != null && minLoadValues.Count > 0)
                {
                    pumpCard.ScaledMinLoad = Int32.Parse(minLoadValues.First());
                }
            }

            var grossStroke = ModbusMessage.PumpLayoutConfig.GrossStroke;
            if (grossStroke != null)
            {
                var grossStrokeValues = GetValueArray(grossStroke.RegisterNumber, grossStroke.NumberOfRegisters, registerValues);
                if (grossStrokeValues != null && grossStrokeValues.Count > 0)
                {
                    pumpCard.GrossStroke = Int32.Parse(grossStrokeValues.First());
                }
            }

            var netStroke = ModbusMessage.PumpLayoutConfig.NetStroke;
            if (netStroke != null)
            {
                var netStrokeValues = GetValueArray(netStroke.RegisterNumber, netStroke.NumberOfRegisters, registerValues);
                if (netStrokeValues != null && netStrokeValues.Count > 0)
                {
                    pumpCard.NetStroke = Int32.Parse(netStrokeValues.First());
                }
            }

            var fluidLoad = ModbusMessage.PumpLayoutConfig.NetStroke;
            if (fluidLoad != null)
            {
                var fluidLoadValues = GetValueArray(fluidLoad.RegisterNumber, fluidLoad.NumberOfRegisters, registerValues);
                if (fluidLoadValues != null && fluidLoadValues.Count > 0)
                {
                    pumpCard.FluidLoad = Int32.Parse(fluidLoadValues.First());
                }
            }

            var pumpFillage = ModbusMessage.PumpLayoutConfig.NetStroke;
            if (pumpFillage != null)
            {
                var pumpFillageValues = GetValueArray(pumpFillage.RegisterNumber, pumpFillage.NumberOfRegisters, registerValues);
                if (pumpFillageValues != null && pumpFillageValues.Count > 0)
                {
                    pumpCard.PumpFillage = Int32.Parse(pumpFillageValues.First());
                }
            }

            var numberOfPointsProp = ModbusMessage.PumpLayoutConfig.NumberOfPoints;
            int numberOfDataPoints = 0;
            if (numberOfPointsProp != null)
            {
                var numberOfPointsValues = GetValueArray(numberOfPointsProp.RegisterNumber, numberOfPointsProp.NumberOfRegisters, registerValues);
                if (numberOfPointsValues != null && numberOfPointsValues.Count > 0)
                {
                    numberOfDataPoints = Int32.Parse(numberOfPointsValues.First());
                    pumpCard.NumberOfPoints = numberOfDataPoints;
                }

                List<CardCoordinate> cardCoordinates = new List<CardCoordinate>();
                var pointArrayProperty = ModbusMessage.PumpLayoutConfig.Point;

                for (int i = 0; i < pumpCard.NumberOfPoints; i += numberOfPointsProp.NumberOfRegisters)
                {
                    var pointsArray = GetValueArray(pointArrayProperty.RegisterNumber + i, pointArrayProperty.NumberOfRegisters, registerValues);
                    if (pointsArray != null && pointsArray.Count > 1)
                    {
                        cardCoordinates.Add(new CardCoordinate()
                        {
                            Load = Int32.Parse(pointsArray[0]),
                            Position = Int32.Parse(pointsArray[1])
                        });
                    }
                }

                pumpCard.CardCoordinates = cardCoordinates;
            }

            return pumpCard;
        }

        private static List<string> GetValueArray(int registerNumber, int length, List<ModbusRegisterValue> valueList)
        {
            List<string> returnValues = new List<string>();
            //System.Console.WriteLine($"Regsiter: {registerNumber}, Length: {length}");

            for (int i = 0; i < length; i++)
            {
                var results = valueList.Where(v => v.Address == (registerNumber + i));

                if (results.Count() == 1)
                {
                    string result = results.First().Value;
                    if (!string.IsNullOrEmpty(result))
                    {
                        returnValues.Add(result);
                        //Console.WriteLine($"Result: {result}");
                    }
                }
                else if (results.Count() > 1)
                {
                    Console.WriteLine($"More than one entry found for register {registerNumber}");
                    foreach (var value in results)
                    {
                        string json = JsonConvert.SerializeObject(value);
                        //Console.WriteLine(json);
                    }
                }
            }

            return returnValues;
        }
    }
}