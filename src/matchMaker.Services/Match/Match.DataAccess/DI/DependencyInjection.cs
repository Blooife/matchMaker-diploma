using Match.DataAccess.Context;
using Match.DataAccess.Providers.Implementations;
using Match.DataAccess.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Match.DataAccess.DI;

public static class DependencyInjection
{
    public static void RegisterDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDbContext(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MatchDatabase:ConnectionString"]!;
        
        services.Configure<MatchDbSettings>(configuration.GetSection("MatchDatabase"));
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddScoped<IMongoDbContext, MatchDbContext>();
    }
}