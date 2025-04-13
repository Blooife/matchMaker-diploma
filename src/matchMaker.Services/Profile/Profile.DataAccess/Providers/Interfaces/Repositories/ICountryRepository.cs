using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface ICountryRepository : IGenericRepository<Country, long>
{
    Task<List<City>> GetAllCitiesFromCountryAsync(long countryId, CancellationToken cancellationToken);
}