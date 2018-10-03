using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynoCardAlertModule.Config
{
    public class PumpCardConfig
    {
        [JsonProperty("timestamp")]
        public ModbusLayoutProperty Timestamp { get; set; }

        [JsonProperty("scaledMaxCardLoad")]
        public ModbusLayoutProperty MaxLoad { get; set; }

        [JsonProperty("scaledMinCardLoad")]
        public ModbusLayoutProperty MinLoad { get; set; }

        [JsonProperty("numberOfPoints")]
        public ModbusLayoutProperty NumberOfPoints { get; set; }

        [JsonProperty("grossStroke")]
        public ModbusLayoutProperty GrossStroke { get; set; }

        [JsonProperty("netStroke")]
        public ModbusLayoutProperty NetStroke { get; set; }

        [JsonProperty("pumpFillage")]
        public ModbusLayoutProperty PumpFillage { get; set; }

        [JsonProperty("fluidLoad")]
        public ModbusLayoutProperty FluidLoad { get; set; }

        [JsonProperty("pointArrayStartRegister")]
        public ModbusLayoutProperty Point { get; set; }
    }
}