using Match.DataAccess.Models;

namespace Match.DataAccess.Providers.Interfaces;

public interface IChatRepository : IGenericRepository<Chat, long>
{
    Task<IEnumerable<Chat>> GetChatsByProfileIdAsync(long profileId, CancellationToken cancellationToken);
    Task<(List<Chat>, int)> GetPagedAsync(long profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
    Task<Chat?> GetChatByProfilesIdsAsync(long firstProfileId, long secondProfileId, CancellationToken cancellationToken);
}