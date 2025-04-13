using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class LanguageRepository(ProfileDbContext _dbContext)
    : GenericRepository<Language, long>(_dbContext), ILanguageRepository
{
    public async Task AddLanguageToProfileAsync(UserProfile profile, Language language)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Languages.Add(language);
    }
    
    public async Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language)
    {
        _dbContext.Profiles.Attach(profile);
        profile.Languages.Remove(language);
    }
}