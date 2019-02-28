using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class SurfaceCard : CardBase
    {
        public int StrokeLength { get; set; }
        public int StrokePeriod { get; set; }

        public SurfaceCard() : base()
        {
            CardType = CardType.Surface;
        }
    }
}