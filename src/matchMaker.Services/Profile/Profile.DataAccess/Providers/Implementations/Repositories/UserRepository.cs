using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class UserRepository(ProfileDbContext _dbContext)
    : GenericRepository<Models.User, long>(_dbContext), IUserRepository
{
    public async Task DeleteUserAsync(Models.User user)
    {
        user.DeletedAt = DateTime.UtcNow;
        _dbContext.Update(user);
    }

    public async Task<Models.User> CreateUserAsync(Models.User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        
        return user;
    }
}