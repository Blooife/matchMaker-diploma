using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Match.DataAccess.Context;

public class MatchDbContext(IOptions<MatchDbSettings> options, IMongoClient client) : IMongoDbContext
{
    private readonly IMongoDatabase _db = client.GetDatabase(options.Value.DatabaseName);

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _db.GetCollection<T>(collectionName);
    }
}