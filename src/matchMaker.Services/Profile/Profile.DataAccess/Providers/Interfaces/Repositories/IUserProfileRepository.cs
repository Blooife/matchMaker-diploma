using System.Linq.Expressions;
using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IUserProfileRepository : IGenericRepository<UserProfile, long>
{
    Task<UserProfile> UpdateProfileAsync(UserProfile profile);
    Task DeleteProfileAsync(UserProfile profile);
    Task<UserProfile> CreateProfileAsync(UserProfile profile, CancellationToken cancellationToken);
    Task<UserProfile?> GetAllProfileInfoAsync(Expression<Func<UserProfile, bool>> expression, CancellationToken cancellationToken);
    Task<IEnumerable<UserProfile>> GetAllProfileInfoByIdsAsync(long[] profileIds);
}