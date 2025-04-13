using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Models;
using User.DataAccess.Providers.Interfaces;

namespace User.DataAccess.Providers.Implementations;

public class RoleProvider(RoleManager<Role> _roleManager) : IRoleProvider
{
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        return await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
    }
}