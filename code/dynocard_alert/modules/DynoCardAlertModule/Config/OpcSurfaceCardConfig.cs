using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OpcSurfaceCardConfig
{ 
    [JsonProperty("numberOfPoints")]
    public string NumberOfPoints { get; set; }

    [JsonProperty("scaledMaxCardLoad")]
    public string MaxLoad { get; set; }

    [JsonProperty("scaledMinCardLoad")]
    public string MinLoad { get; set; }

    [JsonProperty("strokeLength")]
    public string StrokeLength { get; set; }

    [JsonProperty("strokePeriod")]
    public string StrokePeriod { get; set; }

    [JsonProperty("load")]
    public string Load { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }
}