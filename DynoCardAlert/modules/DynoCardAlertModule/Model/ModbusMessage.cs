using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DynoCardAlertModule.Config;

namespace DynoCardAlertModule.Model
{
     public class ModbusMessage
    {
        public string DisplayName { get; set; }
        public List<ModbusRegisterValue> RegisterValues { get; set; }
        public static ModbusLayoutConfig LayoutConfig { get; set; }

        public ModbusMessage(Message message)
        {
            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);
            
            if (!string.IsNullOrEmpty(messageString))
            {
                var registers = JsonConvert.DeserializeObject<List<ModbusRegisterValue>>(messageString);

                if (registers != null && registers.Count > 0)
                {
                    Console.WriteLine("Sending output message");
                    var processedMessage = new Message(messageBytes);
                    RegisterValues = registers;

                    //Preserve the display name off one value to determine what it represents
                    DisplayName = registers[0].DisplayName;

                    Console.WriteLine("Completed output message");
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
        public static DynoCard ToDynoCard(this ModbusMessage message)
        {
            //Convert modbus message to dynocard here
            DynoCard dynoCard = new DynoCard();

            if (!string.IsNullOrEmpty(message.DisplayName))
            {
                if (message.DisplayName.ToLower().Contains("surface"))
                {
                    dynoCard.CardType = DynoCardType.Surface;
                }
                else if (message.DisplayName.ToLower().Contains("pump"))
                {
                    dynoCard.CardType = DynoCardType.Pump;
                }
            } 
            
            var timestampProp = ModbusMessage.LayoutConfig.Timestamp;
            if (timestampProp != null)
            {
                int timestamp = 0;
                var timeStampValues = GetValueArray(timestampProp.RegisterNumber, timestampProp.NumberOfRegisters, message.RegisterValues);

                if (timeStampValues != null && timeStampValues.Count > 1)
                {
                    short left = Int16.Parse(timeStampValues[0]);
                    short right = Int16.Parse(timeStampValues[1]);

                    timestamp = left;
                    timestamp = (timestamp << 16);
                    timestamp = timestamp | (int)(ushort)right;
                }

                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
                dynoCard.Timestamp = dateTime;
            }

            var maxLoadProp = ModbusMessage.LayoutConfig.MaxLoad;
            if (maxLoadProp != null)
            {
                var maxLoadpValues = GetValueArray(maxLoadProp.RegisterNumber, maxLoadProp.NumberOfRegisters, message.RegisterValues);
                if (maxLoadpValues != null && maxLoadpValues.Count > 0)
                {
                    dynoCard.MaxLoad = Int32.Parse(maxLoadpValues.First());
                }
            }

            var minLoadProp = ModbusMessage.LayoutConfig.MinLoad;
            if (minLoadProp != null)
            {
                var minLoadValues = GetValueArray(minLoadProp.RegisterNumber, minLoadProp.NumberOfRegisters, message.RegisterValues);
                if (minLoadValues != null && minLoadValues.Count > 0)
                {
                    dynoCard.MinLoad = Int32.Parse(minLoadValues.First());
                }
            }

            var strokeLengthProp = ModbusMessage.LayoutConfig.StrokeLength;
            if (strokeLengthProp != null)
            {
                var strokeLengthValues = GetValueArray(strokeLengthProp.RegisterNumber, strokeLengthProp.NumberOfRegisters, message.RegisterValues);
                if (strokeLengthValues != null && strokeLengthValues.Count > 0)
                {
                    dynoCard.StrokeLength = Int32.Parse(strokeLengthValues.First());
                }
            }

            var strokePeriodProp = ModbusMessage.LayoutConfig.StrokePeriod;
            if(strokePeriodProp != null)
            {
                var strokePeriodValues = GetValueArray(strokePeriodProp.RegisterNumber, strokePeriodProp.NumberOfRegisters, message.RegisterValues);
                if (strokePeriodValues != null && strokePeriodValues.Count > 0)
                {
                    dynoCard.StrokePeriod = Int32.Parse(strokePeriodValues.First());
                }
            }

            var numberOfPointsProp = ModbusMessage.LayoutConfig.NumberOfPoints;
            int numberOfDataPoints = 0;
            if (numberOfPointsProp != null)
            {
                var numberOfPointsValues = GetValueArray(numberOfPointsProp.RegisterNumber, numberOfPointsProp.NumberOfRegisters, message.RegisterValues);
                if (numberOfPointsValues != null && numberOfPointsValues.Count > 0)
                {
                    numberOfDataPoints = Int32.Parse(numberOfPointsValues.First());
                    dynoCard.NumberOfPoints = numberOfDataPoints;
                }
            }

            var pointsArrayStartProp = ModbusMessage.LayoutConfig.Point;
            if (pointsArrayStartProp != null)
            {
                List<DynoCardPoint> dynoCardPoints = new List<DynoCardPoint>();

                for(int i = 0; i < numberOfDataPoints; i += pointsArrayStartProp.NumberOfRegisters)
                {
                    var pointsArray = GetValueArray(pointsArrayStartProp.RegisterNumber + i, pointsArrayStartProp.NumberOfRegisters, message.RegisterValues);
                    if (pointsArray != null && pointsArray.Count > 1)
                    {
                        dynoCardPoints.Add(new DynoCardPoint()
                        {
                            Load = Int32.Parse(pointsArray[0]),
                            Position = Int32.Parse(pointsArray[1])
                        });
                    }
                }

                dynoCard.CardPoints = dynoCardPoints;
            }

            return dynoCard;           
        }

        private static List<string> GetValueArray(int registerNumber, int length, List<ModbusRegisterValue> valueList)
        {
            List<string> returnValues = new List<string>();
            System.Console.WriteLine($"Regsiter: {registerNumber}, Length: {length}");

            for (int i = 0; i < length; i++)
            {
                string result = valueList.Where(v => v.Address == (registerNumber + i)).SingleOrDefault()?.Value;
                if (!string.IsNullOrEmpty(result))
                {
                    returnValues.Add(result);
                    System.Console.WriteLine($"Result: {result}");
                }
            }

            return returnValues;
        }
    }
}