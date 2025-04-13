using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class InterestRepository(ProfileDbContext _dbContext)
    : GenericRepository<Interest, long>(_dbContext), IInterestRepository
{
    public async Task AddInterestToProfileAsync(UserProfile profile, Interest interest)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Interests.Add(interest);
    }
    
    public async Task RemoveInterestFromProfileAsync(UserProfile profile, Interest interest)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Interests.Remove(interest);
    }
}