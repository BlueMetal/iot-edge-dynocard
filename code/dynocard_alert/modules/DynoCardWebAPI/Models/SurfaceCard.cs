using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class SurfaceCard : CardBase
    {
        public float StrokeLength { get; set; }
        public float StrokePeriod { get; set; }

        public SurfaceCard() : base()
        {
            base.CardType = 0;
        }
    }
}
