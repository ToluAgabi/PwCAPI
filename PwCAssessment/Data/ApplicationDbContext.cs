using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PwCAssessment.Extensions;
using PwCAssessment.Models;

namespace PwCAssessment.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelRequest>()
            .HasIndex(u => u.RequisitionNumber)
            .IsUnique();
        
        modelBuilder.Entity<TravelRequest>()
            .Property(t => t.RequisitionNumber)
            .HasValueGenerator<CustomerNumberGen>();

        base.OnModelCreating(modelBuilder);

        
    }

    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    public DbSet<TravelRequest> TravelRequests => Set<TravelRequest>();

}