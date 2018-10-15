using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OpcPumpCardConfig
{
    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [JsonProperty("scaledMaxCardLoad")]
    public string MaxLoad { get; set; }

    [JsonProperty("scaledMinCardLoad")]
    public string MinLoad { get; set; }

    [JsonProperty("numberOfPoints")]
    public string NumberOfPoints { get; set; }

    [JsonProperty("grossStroke")]
    public string GrossStroke { get; set; }

    [JsonProperty("netStroke")]
    public string NetStroke { get; set; }

    [JsonProperty("pumpFillage")]
    public string PumpFillage { get; set; }

    [JsonProperty("fluidLoad")]
    public string FluidLoad { get; set; }

    [JsonProperty("load")]
    public string Load { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }
}