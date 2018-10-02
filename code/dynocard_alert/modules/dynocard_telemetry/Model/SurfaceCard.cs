using System;
using System.Collections.Generic;

namespace Telemetry.Model
{
    public class SurfaceCard : CardBase
    {
        public int StrokeLength { get; set; }
        public int StrokePeriod { get; set; }

        public SurfaceCard()
        {
            CardType = CardType.Surface;
        }
    }
}