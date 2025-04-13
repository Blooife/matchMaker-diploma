using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface ICityRepository : IGenericRepository<City, long>;