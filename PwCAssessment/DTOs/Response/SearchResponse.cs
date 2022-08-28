namespace PwCAssessment.DTOs.Response;

public class SearchResponse
{
    public string? Country { get; set; }
    public string? Capital { get; set; }
    public List<Weather>? Weather { get; set; }
    public Main? Climate { get; set; }
    public Dictionary<string, Currencies>? Currencies { get; set; }
}