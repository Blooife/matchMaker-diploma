using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class CitiesSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        var countryId = _dbContext.Set<Country>()
            .Where(c => c.Name == "Беларусь")
            .Select(c => c.Id)
            .FirstOrDefault();
        
        foreach (var city in GetCitiesForCountry(countryId))
        {
            CreateIfNotExists(city);
        }
        
        var countryId1 = _dbContext.Set<Country>()
            .Where(c => c.Name == "Россия")
            .Select(c => c.Id)
            .FirstOrDefault();
        
        foreach (var city in GetCitiesForRussia(countryId1))
        {
            CreateIfNotExists(city);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(City city)
    {
        var hasAny = _dbContext.Set<City>().Any(x => x.Name == city.Name);
        if (!hasAny)
        {
            _dbContext.Set<City>().Add(city);
        }
    }
    
    private IEnumerable<City> GetCitiesForCountry(long countryId)
    {
        yield return Create("Пинск");
        yield return Create("Минск");
        yield return Create("Брест");
        yield return Create("Гомель");
        yield return Create("Гродно");
        yield return Create("Витебск");
        yield return Create("Могилёв");
        yield break;

        City Create(string name)
        {
            return new City()
            {
                Name = name,
                CountryId = countryId
            };
        }
    }
    
    private IEnumerable<City> GetCitiesForRussia(long countryId)
    {
        yield return Create("Москва");
        yield return Create("Санкт-Петербург");
        yield return Create("Самара");
        yield return Create("Волгоград");
        yield return Create("Краснодар");
        yield return Create("Воронеж");
        yield return Create("Пенза");
        yield break;

        City Create(string name)
        {
            return new City()
            {
                Name = name,
                CountryId = countryId
            };
        }
    }
}