using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynoCardAlertModule.Config
{
    public class ModbusLayoutConfig
    {
        [JsonProperty("surfaceCardConfig")]
        public SurfaceCardConfig SurfaceCardConfiguration { get; set; }

        [JsonProperty("pumpCardConfig")]
        public PumpCardConfig PumpCardConfiguration { get; set; }
    }
}
