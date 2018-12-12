using System;
using System.Collections.Generic;

namespace Telemetry.Model
{
    public class StaticDataContainer
    {
        public int[] SurfaceLoadValues { get; set; }
        public int[] SurfacePositionValues { get; set; }
        public int[] PumpLoadValues { get; set; }
        public int[] PumpPositionValues { get; set; }
    }
}