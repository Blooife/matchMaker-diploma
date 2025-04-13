using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.DataAccess.Providers.Interfaces;

public interface IProfileRepository : IGenericRepository<Profile, long>
{
    Task<List<long>> GetRecsAsync(List<long> excludedProfileIds, Profile userProfile, CancellationToken cancellationToken);
}