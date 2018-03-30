using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynoCardAlertModule.Config
{
    public class ModbusLayoutConfig
    {
        [JsonProperty("timestamp")]
        public ModbusLayoutProperty Timestamp { get; set; }

        [JsonProperty("numberOfPoints")]
        public ModbusLayoutProperty NumberOfPoints { get; set; }

        [JsonProperty("scaledMaxCardLoad")]
        public ModbusLayoutProperty MaxLoad { get; set; }

        [JsonProperty("scaledMinCardLoad")]
        public ModbusLayoutProperty MinLoad { get; set; }

        [JsonProperty("strokeLength")]
        public ModbusLayoutProperty StrokeLength { get; set; }

        [JsonProperty("strokePeriod")]
        public ModbusLayoutProperty StrokePeriod { get; set; }

        [JsonProperty("pointArrayStartRegister")]
        public ModbusLayoutProperty Point { get; set; }
    }
}
