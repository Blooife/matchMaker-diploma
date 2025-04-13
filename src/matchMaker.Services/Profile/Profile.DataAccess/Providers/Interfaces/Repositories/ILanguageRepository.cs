using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface ILanguageRepository : IGenericRepository<Language, long>
{
    Task AddLanguageToProfileAsync(UserProfile profile, Language language);
    Task RemoveLanguageFromProfileAsync(UserProfile profile, Language language);
}