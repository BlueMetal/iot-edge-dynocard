using System;
using System.Collections.Generic;

namespace DynoCardAlertModule.Model
{
    public class DynoCard
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int NumberOfPoints { get; set; }
        public int MaxLoad { get; set; }
        public int MinLoad { get; set; }
        public int StrokeLength { get; set; }
        public int StrokePeriod { get; set; }
        public List<DynoCardPoint> CardPoints { get; set; }
    }
}