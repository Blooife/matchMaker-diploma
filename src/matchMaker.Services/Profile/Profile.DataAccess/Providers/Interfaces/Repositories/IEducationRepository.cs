using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IEducationRepository : IGenericRepository<Education, long>
{
    Task AddEducationToProfileAsync(UserProfile profile, ProfileEducation user);
    Task RemoveEducationFromProfileAsync(UserProfile profile, ProfileEducation user);
    Task UpdateProfilesEducationAsync(ProfileEducation user, string description);
}