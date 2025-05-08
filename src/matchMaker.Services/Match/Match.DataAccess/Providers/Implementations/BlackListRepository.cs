using Common.Exceptions;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class BlackListRepository(IMongoCollection<BlackList> _collection) : GenericRepository<BlackList, string>(_collection), IBlackListRepository
{
    public async Task CheckCanSendMessageAsync(long senderId, long receiverId)
    {
        var filter = Builders<BlackList>.Filter.Or(
            Builders<BlackList>.Filter.And(
                Builders<BlackList>.Filter.Eq(b => b.BlockerProfileId, receiverId),
                Builders<BlackList>.Filter.Eq(b => b.BlockedProfileId, senderId)
            ),
            Builders<BlackList>.Filter.And(
                Builders<BlackList>.Filter.Eq(b => b.BlockerProfileId, senderId),
                Builders<BlackList>.Filter.Eq(b => b.BlockedProfileId, receiverId)
            )
        );

        var blocked = await _collection.Find(filter).FirstOrDefaultAsync();

        if (blocked != null)
        {
            if (blocked.BlockerProfileId == receiverId && blocked.BlockedProfileId == senderId)
            {
                throw new ConflictException("Вы не можете отправить сообщение: получатель добавил вас в чёрный список.");
            }
            if (blocked.BlockerProfileId == senderId && blocked.BlockedProfileId == receiverId)
            {
                throw new ConflictException("Вы не можете отправить сообщение пользователю, которого сами заблокировали.");
            }

            throw new ConflictException("Отправка сообщения невозможна из-за блокировки.");
        }
    }
    
    public async Task<bool> ExistsAsync(long blockerUserId, long blockedUserId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BlackList>.Filter.And(
            Builders<BlackList>.Filter.Eq(b => b.BlockerProfileId, blockerUserId),
            Builders<BlackList>.Filter.Eq(b => b.BlockedProfileId, blockedUserId)
        );

        var count = await _collection.CountDocumentsAsync(ApplySoftDeleteFilter(filter), cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<List<BlackList>> GetByBlockerIdAsync(long blockerUserId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BlackList>.Filter.Eq(b => b.BlockerProfileId, blockerUserId);
        return await _collection.Find(ApplySoftDeleteFilter(filter)).ToListAsync(cancellationToken);
    }
    
    public async Task<List<BlackList>> GetByBlockedIdAsync(long blockedUserId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BlackList>.Filter.Eq(b => b.BlockedProfileId, blockedUserId);
        return await _collection.Find(ApplySoftDeleteFilter(filter)).ToListAsync(cancellationToken);
    }

    public async Task RemoveAsync(long blockerUserId, long blockedUserId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BlackList>.Filter.And(
            Builders<BlackList>.Filter.Eq(b => b.BlockerProfileId, blockerUserId),
            Builders<BlackList>.Filter.Eq(b => b.BlockedProfileId, blockedUserId)
        );

        await _collection.DeleteOneAsync(filter, cancellationToken);
    }
}