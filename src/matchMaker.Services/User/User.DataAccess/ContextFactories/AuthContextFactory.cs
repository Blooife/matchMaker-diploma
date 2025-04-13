using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using User.DataAccess.Contexts;

namespace User.DataAccess.ContextFactories;

internal class AuthContextFactory : IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(basePath, "../User.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<UserContext>()
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new UserContext(builder.Options);
    }
}