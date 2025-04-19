using System.Linq.Expressions;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class MessageRepository(IMongoCollection<Message> _collection) : GenericRepository<Message, string>(_collection), IMessageRepository
{
    public async Task<(List<Message>, int)> GetPagedAsync(string chatId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<Message, bool>> filter = x => x.ChatId == chatId;

        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Message, Message>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<Message>.Sort.Descending(m => m.Timestamp)
        };

        var items = await _collection.Find(filter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new (items, (int)count);
    }
}