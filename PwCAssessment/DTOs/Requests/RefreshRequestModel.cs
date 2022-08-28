using System.ComponentModel.DataAnnotations;

namespace PwCAssessment.DTOs.Requests;

public class RefreshRequestModel
{
    [Required]
    public string? AuthenticationToken { get; set; }
    [Required]
    public string? RefreshToken { get; set; }
}