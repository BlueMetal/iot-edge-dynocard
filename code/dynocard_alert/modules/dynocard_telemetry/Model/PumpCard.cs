using System;
using System.Collections.Generic;

namespace Telemetry.Model
{
    public class PumpCard : CardBase
    {
        public int NetStroke { get; set; }
        public int GrossStroke { get; set; }
        public int PumpFillage { get; set; }
        public int FluidLoad { get; set; }

        public PumpCard()
        {
            CardType = CardType.Pump;
        }
    }
}