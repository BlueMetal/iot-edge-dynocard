using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class CardBase
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int NumberOfPoints { get; set; }
        public int ScaledMaxLoad { get; set; }
        public int ScaledMinLoad { get; set; }
        public CardType CardType { get; set; }
        public List<CardCoordinate> CardCoordinates { get; set; }

        public CardBase()
        {
            CardCoordinates = new List<CardCoordinate>();
        }
    }
}