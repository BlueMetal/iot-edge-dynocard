using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynoCardAlertModule.Config
{
    public class ModbusLayoutProperty
    {
        [JsonProperty("register")]
        public int RegisterNumber { get; set; }
        [JsonProperty("length")]
        public int NumberOfRegisters { get; set; }
    }
}
