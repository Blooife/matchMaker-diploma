using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Contexts;

public class ProfileDbContext : DbContext
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options) { }
    
    public DbSet<UserProfile> Profiles { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Models.User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProfileEducation>()
            .HasKey(userEducation => new { userEducation.ProfileId, userEducation.EducationId });
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}