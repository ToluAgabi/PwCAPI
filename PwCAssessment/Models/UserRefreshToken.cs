using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PwCAssessment.Models;

public class UserRefreshToken
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? RefreshToken { get; set; }
}