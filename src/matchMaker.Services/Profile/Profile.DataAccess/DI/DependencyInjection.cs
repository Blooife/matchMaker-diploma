using Common.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Providers.Implementations.Repositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Profile.DataAccess.Seeds;

namespace Profile.DataAccess.DI;

public static class DependencyInjection
{
    public static void RegisterDataAccess(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureDbContext(config);
        services.ConfigureRepositories();
        services.RegisterSeeds();
    }
    
    private static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProfileDbContext>(options => options.UseNpgsql(connectionString));
    }
    
    private static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IInterestRepository, InterestRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void RegisterSeeds(this IServiceCollection services)
    {
        
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, CountriesSeeds>();
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, CitiesSeeds>();
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, EducationsSeeds>();
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, GoalsSeeds>();
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, LanguagesSeeds>();
        services.AddTransient<ISeedEntitiesProvider<ProfileDbContext>, InterestsSeeds>();
    }
}