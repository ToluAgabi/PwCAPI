using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;

namespace PwCAssessment.DTOs;

public class GenericResponse<T>
{
    [DefaultValue(true)] 
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
}

public class ErrorResponse
{
   [DefaultValue(false)]
    public  bool Success { get; set; } = false;

    [DefaultValue(null)]
    public  object? Data { get; set; }
    public string? Message { get; set; }


}

