using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class PumpCard : CardBase
    {
        public float GrossStroke { get; set; }
        public float NetStroke { get; set; }
        public float PumpFillage { get; set; }
        public float FluidLoad { get; set; }

        public PumpCard() : base()
        {
            base.CardType = 1;
        }
    }
}
