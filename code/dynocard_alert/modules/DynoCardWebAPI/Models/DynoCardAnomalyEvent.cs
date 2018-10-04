using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class DynoCardAnomalyEvent
    {
        public Guid AnomalyId { get; set; } // Correlates all dyno cards for this anomaly
        public string Timestamp { get; set; }  // Timestamp of the anomaly
        public int PumpId { get; set; } // Pump ID that generated the anomaly
        public DynoCard DynoCard = new DynoCard();
    }
}