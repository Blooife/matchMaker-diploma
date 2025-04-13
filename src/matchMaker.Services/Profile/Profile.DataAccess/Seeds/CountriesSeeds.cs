using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class CountriesSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        foreach (var city in GetCountries())
        {
            CreateIfNotExists(city);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Country country)
    {
        var hasAny = _dbContext.Set<Country>().Any(x => x.Name == country.Name);
        if (!hasAny)
        {
            _dbContext.Set<Country>().Add(country);
        }
    }
    
    private IEnumerable<Country> GetCountries()
    {
        yield return Create("Беларусь");
        yield return Create("Россия");
        yield return Create("Польша");
        yield return Create("Казахстан");
        yield break;

        Country Create(string name)
        {
            return new Country()
            {
                Name = name,
            };
        }
    }
}