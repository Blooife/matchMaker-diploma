using Match.DataAccess.Models;

namespace Match.DataAccess.Providers.Interfaces;

public interface IBlackListRepository : IGenericRepository<BlackList, string>
{
    Task CheckCanSendMessageAsync(long senderId, long receiverId);
    Task<bool> ExistsAsync(long blockerUserId, long blockedUserId, CancellationToken cancellationToken = default);
    Task<List<BlackList>> GetByBlockerIdAsync(long blockerUserId, CancellationToken cancellationToken = default);
    Task RemoveAsync(long blockerUserId, long blockedUserId, CancellationToken cancellationToken = default);
    Task<List<BlackList>> GetByBlockedIdAsync(long blockedUserId, CancellationToken cancellationToken = default);
}