using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynoCardAlertModule.Model
{
    public class DynoCard
    {
        public DynoCard()
        {
            SurfaceCard = new SurfaceCard();
            PumpCard = new PumpCard();
        }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("surfaceCard")]
        public SurfaceCard SurfaceCard { get; set; }

        [JsonProperty("pumpCard")]
        public PumpCard PumpCard { get; set; }

        //Bool flag indicating if this card was the anomaly that triggered the alert
        [JsonProperty("triggeredEvents")]
        public bool TriggeredEvents { get; set; }
    }
}