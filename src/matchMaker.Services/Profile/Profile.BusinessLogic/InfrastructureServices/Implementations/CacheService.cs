using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Caching.Distributed;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;

namespace Profile.BusinessLogic.InfrastructureServices.Implementations;

public class CacheService(IDistributedCache _distributedCache) : ICacheService
{
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromHours(1);
    private readonly JsonSerializerOptions _jsonSerializerOptions = new ()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        return cachedValue is null ? null : JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);
    }
    
    public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        if (cachedValue is null)
        {
            return null;
        }
        
        await SetAsync(key, cachedValue, cancellationToken:cancellationToken);

        return cachedValue;
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value, _jsonSerializerOptions);

        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = absoluteExpiration ?? _defaultExpiration;
        
        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}