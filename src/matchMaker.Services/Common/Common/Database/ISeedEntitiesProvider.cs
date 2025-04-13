namespace Common.Database;

public interface ISeedEntitiesProvider<TContext> where TContext : class
{
    Task SeedAsync();
}