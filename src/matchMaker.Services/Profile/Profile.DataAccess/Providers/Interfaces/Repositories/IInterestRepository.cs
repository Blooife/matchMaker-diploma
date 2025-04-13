using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IInterestRepository : IGenericRepository<Interest, long>
{
    Task AddInterestToProfileAsync(UserProfile profile, Interest interest);
    Task RemoveInterestFromProfileAsync(UserProfile profile, Interest interest);
}
