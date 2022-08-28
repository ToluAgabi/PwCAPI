
using Newtonsoft.Json;

namespace PwCAssessment.DTOs.Response;

public partial class LocationResponse
{
    [JsonProperty("capitalInfo")]
    public CapitalInfo? CapitalInfo { get; set; }

    [JsonProperty("name")]
    public Name? Name { get; set; }

    [JsonProperty("capital")]
    public List<string>? Capital { get; set; }
    
    [JsonProperty("currencies")]
    public Dictionary<string, Currencies>? Currencies { get; set; }
}

public partial class CapitalInfo
{
    [JsonProperty("latlng")]
    public List<double>? Latlng { get; set; }
}

public partial class Name
{
    [JsonProperty("common")]
    public string? Common { get; set; }

    [JsonProperty("official")]
    public string? Official { get; set; }

    [JsonProperty("nativeName")]
    public Dictionary<string, NativeName>? NativeName { get; set; }
    
  
}

public partial class NativeName
{
    [JsonProperty("official")]
    public string? Official { get; set; }

    [JsonProperty("common")]
    public string? Common { get; set; }
}

public partial class Currencies
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("symbol")]
    public string? Symbol { get; set; }
}


