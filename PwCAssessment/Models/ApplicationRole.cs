using Microsoft.AspNetCore.Identity;

namespace PwCAssessment.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole(string roleName) : base(roleName)
    {
    }
    public ApplicationRole()
    {

    }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }

}