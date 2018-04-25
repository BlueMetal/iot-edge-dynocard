using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class DynoCard
    {
        public string Timestamp { get; set; } // Timestamp that the Dyno Card was generated
        public bool TriggeredEvents { get; set; } // True when this Dyno Card triggered the anomaly
        public SurfaceCard surfaceCard = new SurfaceCard();
        public PumpCard pumpCard = new PumpCard();
    }
}
