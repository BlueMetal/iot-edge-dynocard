using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class DynoCardAnomalyEvent
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("pump")]
        public int Pump { get; set; }

        [JsonProperty("dynoCards")]
        public List<DynoCard> DynoCards { get; set; }
    }
}