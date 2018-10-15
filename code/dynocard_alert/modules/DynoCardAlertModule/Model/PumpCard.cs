using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class PumpCard : CardBase
    {
        public int NetStroke { get; set; }
        public int GrossStroke { get; set; }
        public int PumpFillage { get; set; }
        public int FluidLoad { get; set; }

        public PumpCard() : base()
        {
            CardType = CardType.Pump;
        }
    }
}