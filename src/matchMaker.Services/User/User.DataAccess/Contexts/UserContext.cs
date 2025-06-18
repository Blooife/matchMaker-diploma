using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Models;

namespace User.DataAccess.Contexts;

public class UserContext : IdentityDbContext<Models.User, Role, long>
{
    public DbSet<UserReport> UserReports { get; set; } = null!;
    public DbSet<ReportType> ReportTypes { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options) : base(options){ }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Models.User>(b =>
        {
            b.ToTable("Users");
        });
        
        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable("Roles");
        });
    }
}