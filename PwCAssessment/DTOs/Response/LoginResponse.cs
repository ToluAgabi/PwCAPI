namespace PwCAssessment.DTOs.Response;

public class LoginResponse
{
    public string? UserId { get; set; }
    public string? OrganisationId { get; set; }
    public string? Email { get; set; }
    public string? Firstname { get; set; }
    public string? LastName { get; set; }
    public string? UserToken { get; set; }
    public string? RefreshToken { get; set; }

}