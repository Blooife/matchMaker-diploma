using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class CityRepository(ProfileDbContext _dbContext)
    : GenericRepository<City, long>(_dbContext), ICityRepository
{
}