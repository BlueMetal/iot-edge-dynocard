using System;
using System.Collections.Generic;

namespace Telemetry.Model
{
    public interface IDynocardDataSnapshot
    {
        AnomalyType AnomalyType { get; set; }
        int[] SurfaceLoadValues { get; set; }
        int[] SurfacePositionValues { get; set; }
        int[] PumpLoadValues { get; set; }
        int[] PumpPositionValues { get; set; }
    }
}