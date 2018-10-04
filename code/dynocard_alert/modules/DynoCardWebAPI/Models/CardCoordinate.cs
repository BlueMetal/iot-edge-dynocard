using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynoCardWebAPI.Models
{
    public class CardCoordinate
    {
        public float Position { get; set; }
        public float Load { get; set; }
        public int Order { get; set; }

        public CardCoordinate(int order, float position, float load)
        {
            this.Position = position;
            this.Load = load;
            this.Order = order;
        }
    }
}
