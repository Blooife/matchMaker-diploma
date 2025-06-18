using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class InterestsSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        foreach (var interest in GetInterests())
        {
            CreateIfNotExists(interest);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Interest interest)
    {
        var hasAny = _dbContext.Set<Interest>().Any(x => x.Name == interest.Name);
        if (!hasAny)
        {
            _dbContext.Set<Interest>().Add(interest);
        }
    }
    
    private IEnumerable<Interest> GetInterests()
    {
        yield return Create("Музыка");
        yield return Create("Фильмы");
        yield return Create("Футбол");
        yield return Create("Спорт");
        yield return Create("Мода");
        yield return Create("Программирование");
        yield return Create("Театр");
        yield return Create("Языки");
        yield return Create("Культура");
        yield return Create("Велосипед");
        yield return Create("Танцы");
        yield break;

        Interest Create(string name)
        {
            return new Interest()
            {
                Name = name,
            };
        }
    }
}