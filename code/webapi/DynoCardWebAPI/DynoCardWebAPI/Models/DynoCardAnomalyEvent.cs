using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class DynoCardAnomalyEvent
    {
        public int PumpId { get; set; }
        public int Epoch { get; set; }
        public List<DynoCard> dynoCards = new List<DynoCard>();
    }
}