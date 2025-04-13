using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.DataAccess.Providers.Interfaces;

public interface IMatchRepository : IGenericRepository<MatchEntity, long>
{
    Task<bool> AreProfilesMatchedAsync(long profileId1, long profileId2, CancellationToken cancellationToken);
    Task<(List<MatchEntity>, int)> GetPagedAsync(long profileId, int pageNumber, int pageSize,
        CancellationToken cancellationToken);
}