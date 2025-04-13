using MongoDB.Driver;

namespace Match.DataAccess.Context;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}