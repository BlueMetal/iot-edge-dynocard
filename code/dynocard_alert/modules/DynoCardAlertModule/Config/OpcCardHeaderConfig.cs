using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class OpcCardHeaderConfig
{
    [JsonProperty("cardType")]
    public string CardType { get; set; }

    [JsonProperty("cardID")]
    public string CardID { get; set; }
}