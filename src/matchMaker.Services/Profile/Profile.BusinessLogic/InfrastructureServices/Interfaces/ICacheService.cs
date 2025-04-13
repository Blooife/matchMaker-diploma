namespace Profile.BusinessLogic.InfrastructureServices.Interfaces;

public interface ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;
    public Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        where T : class;
    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
        where T : class;
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}