using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OpcMessageConfig
{
    [JsonProperty("opcPumpCardConfig")]
    public OpcSurfaceCardConfig SurfaceCard { get; set; }

    [JsonProperty("opcSurfaceCardConfig")]
    public OpcPumpCardConfig PumpCard { get; set; }
}