using Common.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.DataAccess.Contexts;
using User.DataAccess.Models;
using User.DataAccess.Providers.Implementations;
using User.DataAccess.Providers.Interfaces;
using User.DataAccess.Seeds;

namespace User.DataAccess.DI;

public static class UserDiRegistration
{
    public static void RegisterDataAccess(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureDbContext(config);
        services.RegisterIdentity();
        services.RegisterProviders();
        services.AddTransient<ISeedEntitiesProvider<UserContext>, RolesSeeds>();
        services.AddTransient<ISeedEntitiesProvider<UserContext>, UsersSeeds>();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<UserContext>(options => options.UseNpgsql(connectionString));
    }
    
    private static void RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<Models.User, Role>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();
    }
    
    private static void RegisterProviders(this IServiceCollection services)
    {
        services.AddScoped<IUserProvider, UserProvider>();
        services.AddScoped<IRoleProvider, RoleProvider>();
    }
}