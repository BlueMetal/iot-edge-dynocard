using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class CardBase
    {
        public double Epoch { get; set; }
        public int NumPoints { get; set; }
        public float ScaledMaxLoad { get; set; }
        public float ScaledMinLoad { get; set; }
        public List<CardCoordinate> cardCoordinates = new List<CardCoordinate>();
        public string CardType { get; set; }
    }
}
