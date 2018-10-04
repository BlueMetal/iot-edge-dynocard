using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class CardBase
    {
        public string Timestamp { get; set; }
        public int NumPoints { get; set; }
        public float ScaledMaxLoad { get; set; }
        public float ScaledMinLoad { get; set; }
        public List<CardCoordinate> cardCoordinates;
        public int CardType { get; set; }

        public CardBase()
        {
            cardCoordinates = new List<CardCoordinate>(5);
        }
    }
}
