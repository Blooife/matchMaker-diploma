using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<Models.User, long>
{
    Task DeleteUserAsync(Models.User user);
    Task<Models.User> CreateUserAsync(Models.User user, CancellationToken cancellationToken);
}