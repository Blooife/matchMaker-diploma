using Microsoft.AspNetCore.Identity;

namespace User.DataAccess.Providers.Interfaces;

public interface IUserProvider
{
    Task<Models.User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Models.User?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IdentityResult> RegisterAsync(Models.User user, string? password);
    Task<bool> CheckPasswordAsync(Models.User user, string password);
    Task<IList<string>> GetRolesAsync(Models.User user);
    Task<IdentityResult> AddToRoleAsync(Models.User user, string roleName);
    Task<IdentityResult> RemoveFromRoleAsync(Models.User user, string roleName);
    Task<Models.User?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteUserByIdAsync(Models.User user);
    Task<IdentityResult> UpdateUserAsync(Models.User user);
    Task<(List<Models.User> Users, int TotalCount)> GetPagedUsersAsync(int pageNumber, int pageSize);
    Task BanUserAsync(long userId, DateTime? bannedUntil = null, CancellationToken cancellationToken = default);
    Task<IdentityResult> UpdatePasswordAsync(DataAccess.Models.User user, string currentPassword, string newPassword);
}