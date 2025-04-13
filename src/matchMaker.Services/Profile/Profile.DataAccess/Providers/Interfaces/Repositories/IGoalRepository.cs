using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IGoalRepository : IGenericRepository<Goal, long>;