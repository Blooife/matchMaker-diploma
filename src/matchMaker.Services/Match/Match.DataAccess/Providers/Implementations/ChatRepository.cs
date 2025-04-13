using System.Linq.Expressions;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class ChatRepository(IMongoCollection<Chat> _collection) : GenericRepository<Chat, long>(_collection), IChatRepository
{
    public async Task<IEnumerable<Chat>> GetChatsByProfileIdAsync(long profileId, CancellationToken cancellationToken)
    {
        var chats = await GetAsync(chat => chat.FirstProfileId == profileId || chat.SecondProfileId == profileId, cancellationToken);
        
        return chats;
    }
    
    public async Task<(List<Chat>, int)> GetPagedAsync(long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        Expression<Func<Chat, bool>> filter = chat => chat.FirstProfileId == profileId || chat.SecondProfileId == profileId;
        
        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var findOptions = new FindOptions<Chat, Chat>()
        {
            Skip = (pageNumber - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<Chat>.Sort.Descending(chat => chat.LastMessageTimestamp)
        };

        var items = await _collection.Find(filter)
            .Sort(findOptions.Sort)
            .Skip(findOptions.Skip)
            .Limit(findOptions.Limit)
            .ToListAsync(cancellationToken);
        
        return new (items, (int)count);
    }
    
    public async Task<Chat?> GetChatByProfilesIdsAsync(long firstProfileId, long secondProfileId, CancellationToken cancellationToken)
    {
        var filter = Builders<Chat>.Filter.Or(
            Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.Eq(c => c.FirstProfileId, firstProfileId),
                Builders<Chat>.Filter.Eq(c => c.SecondProfileId, secondProfileId)
            ),
            Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.Eq(c => c.FirstProfileId, secondProfileId),
                Builders<Chat>.Filter.Eq(c => c.SecondProfileId, firstProfileId)
            )
        );
        
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}