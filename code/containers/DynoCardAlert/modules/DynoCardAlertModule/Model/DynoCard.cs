using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class DynoCard
    {
        public int Id { get; set; }

        public SurfaceCard SurfaceCard { get; set; }

        public PumpCard PumpCard { get; set; }

        //Bool flag indicating if this card was the anomaly that triggered the alert
        public bool TriggeredEvents { get; set; }

        public DateTime Timestamp { get; set; }

        public int Pump { get; set; }
    }
}