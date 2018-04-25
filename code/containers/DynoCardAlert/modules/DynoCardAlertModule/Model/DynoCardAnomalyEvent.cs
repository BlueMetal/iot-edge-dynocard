using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class DynoCardAnomalyEvent
    {
        [JsonProperty("anomalyId")]
        public Guid AnomalyId { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("pumpId")]
        public int PumpId { get; set; }

        [JsonProperty("dynoCard")]
        public DynoCard DynoCard { get; set; }
    }
}