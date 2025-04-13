using Microsoft.EntityFrameworkCore;
using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class CountryRepository(ProfileDbContext _dbContext)
    : GenericRepository<Country, long>(_dbContext), ICountryRepository
{
    public async Task<List<City>> GetAllCitiesFromCountryAsync(long countryId, CancellationToken cancellationToken)
    {
        var countryWithCities = await 
            _dbContext.Countries.Include(c => c.Cities).AsNoTracking()
                .FirstOrDefaultAsync(c=>c.Id == countryId, cancellationToken);
        
        return countryWithCities!.Cities;
    }
}