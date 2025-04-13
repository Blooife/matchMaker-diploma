using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class LanguagesSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        foreach (var language in GetLanguages())
        {
            CreateIfNotExists(language);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Language language)
    {
        var hasAny = _dbContext.Set<Language>().Any(x => x.Name == language.Name);
        if (!hasAny)
        {
            _dbContext.Set<Language>().Add(language);
        }
    }
    
    private IEnumerable<Language> GetLanguages()
    {
        yield return Create("Белорусский");
        yield return Create("Русский");
        yield return Create("Английский");
        yield return Create("Немецкий");
        yield return Create("Французский");
        yield return Create("Итальянский");
        yield return Create("Японский");
        yield return Create("Испанский");
        yield break;

        Language Create(string name)
        {
            return new Language()
            {
                Name = name,
            };
        }
    }
}