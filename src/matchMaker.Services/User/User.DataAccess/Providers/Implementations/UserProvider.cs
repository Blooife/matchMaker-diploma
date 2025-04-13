using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Contexts;
using User.DataAccess.Providers.Interfaces;

namespace User.DataAccess.Providers.Implementations;

public class UserProvider(UserContext _dbContext, UserManager<Models.User> _userManager) : IUserProvider
{
     public async Task<Models.User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IdentityResult> RegisterAsync(Models.User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(Models.User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(Models.User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddToRoleAsync(Models.User user, string roleName)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }
    
    public async Task<IdentityResult> RemoveFromRoleAsync(Models.User user, string roleName)
    {
        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<Models.User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);
    }

    public async Task<Models.User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.DeletedAt == null).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
    
    public async Task<IdentityResult> UpdateUserAsync(Models.User user)
    {
        return await _userManager.UpdateAsync(user);
    }
    
    public async Task<IdentityResult> DeleteUserByIdAsync(Models.User user)
    {
        user.DeletedAt = DateTime.UtcNow;
        
        return await _userManager.UpdateAsync(user);
    }
    
    public async Task<(List<Models.User> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize)
    {
        var query = _dbContext.Set<Models.User>().Where(u => u.DeletedAt == null);
        var totalCount = query.Count();

        var users = await query
            .OrderBy(e => e.Email)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (users, totalCount);
    }
}