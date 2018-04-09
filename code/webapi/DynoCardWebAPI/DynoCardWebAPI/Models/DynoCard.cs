using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class DynoCard
    {
        public bool TriggeredEvents { get; set; }
        public SurfaceCard surfaceCard = new SurfaceCard();
        public PumpCard pumpCard = new PumpCard();
    }
}
