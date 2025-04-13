using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Database;

public static class MigrateServiceProviderExtensions
{
    public static async Task ExecuteSeeds<TContext>(this IServiceProvider scope) where TContext : DbContext
    {
        var logger = scope.GetRequiredService<ILogger<TContext>>();

        try
        {
            var seedProviders = scope.GetServices<ISeedEntitiesProvider<TContext>>();
            foreach (var seedProvider in seedProviders)
            {
                await seedProvider.SeedAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An error occurred while seed the database used on context {Name}",
                typeof(TContext).Name);
        }
    }
}