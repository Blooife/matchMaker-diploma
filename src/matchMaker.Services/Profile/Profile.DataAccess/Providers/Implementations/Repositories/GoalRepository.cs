using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class GoalRepository(ProfileDbContext _dbContext)
    : GenericRepository<Goal, long>(_dbContext), IGoalRepository
{

}