using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class EducationRepository(ProfileDbContext _dbContext)
    : GenericRepository<Education, long>(_dbContext), IEducationRepository
{
    public async Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation user)
    {
        _dbContext.Profiles.Attach(profile);
        profile.ProfileEducations.Add(user);
    }
    
    public async Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation user)
    {
        _dbContext.Profiles.Attach(profile);
        profile.ProfileEducations.Remove(user);
    }
    
    public async Task UpdateProfilesEducationAsync(ProfileEducation user, string description)
    {
        _dbContext.Attach(user);
        user.Description = description;
    }
}