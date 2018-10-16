using System;
using System.Text;

namespace DynoCardAlertModule.Model
{
    public class ModbusRegisterValue
    {
        public string DisplayName { get; set; }
        public int Address { get; set; }
        public string Value { get; set; }
    }
}