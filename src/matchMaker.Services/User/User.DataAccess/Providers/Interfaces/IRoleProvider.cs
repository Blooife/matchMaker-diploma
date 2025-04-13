using User.DataAccess.Models;

namespace User.DataAccess.Providers.Interfaces;

public interface IRoleProvider
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken);
}