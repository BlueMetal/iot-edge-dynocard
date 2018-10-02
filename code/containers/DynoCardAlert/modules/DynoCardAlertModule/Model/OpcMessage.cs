using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class OpcMessage
    {
        public OpcMessage(Message message)
        {
            var messageBytes = message.GetBytes();
            var messageString = Encoding.UTF8.GetString(messageBytes);

            if (!string.IsNullOrEmpty(messageString))
            {
                // var registers = JsonConvert.DeserializeObject<List<ModbusRegisterValue>>(messageString);

                // if (registers != null && registers.Count > 0)
                // {
                //     //Sort the list so we know what the first Op name is
                //     registers = registers.OrderBy(r => r.Address).ToList();

                //     SurfaceCardRegisterValues = FilterSurfaceCardRegisters(registers);
                //     PumpCardRegisterValues = FilterPumpCardRegisters(registers);

                //     Console.WriteLine("Completed creating modbus message");
                // }
                // else
                // {
                //     Console.WriteLine("Empty list of register values");
                // }
            }
            else
            {
                Console.WriteLine("Empty modbus message received");
            }
        }
    }
}