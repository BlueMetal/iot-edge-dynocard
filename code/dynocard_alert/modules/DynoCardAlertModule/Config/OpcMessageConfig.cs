using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OpcMessageConfig
{
    public OpcCardHeaderConfig Header { get; set; }
    public OpcSurfaceCardConfig SurfaceCard { get; set; }
    public OpcPumpCardConfig PumpCard { get; set; }
}