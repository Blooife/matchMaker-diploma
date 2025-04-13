using Common.Database;
using Microsoft.AspNetCore.Identity;
using User.DataAccess.Contexts;
using User.DataAccess.Models;

namespace User.DataAccess.Seeds;

public class UsersSeeds(UserContext _dbContext, UserManager<Models.User> _userManager) : ISeedEntitiesProvider<UserContext>
{
    public async Task SeedAsync()
    {
        var users = GetUsers();
        foreach (var u in users)
        {
            await CreateIfNotExistsAsync(u.Item1, u.Item2, u.Item3);
        }
    }
    
    private async Task CreateIfNotExistsAsync(string email, string password, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null) return;

        user = new Models.User
        {
            Email = email,
            UserName = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
    
    private IEnumerable<(string, string, string)> GetUsers()
    {
        yield return ("user@gmail.com", "user123", "User");
        yield return ("admin@gmail.com", "admin123", "Admin");
        yield return ("moderator@gmail.com", "moderator123", "Moderator");
        yield break;

        Models.User Create(string email, string password, string roleName)
        {
            var role = _dbContext.Roles.FirstOrDefault(x => x.Name == roleName);
            var user = new Models.User()
            {
                Email = email,
                Roles = new List<Role>()
                {
                    role
                }
            };
            PasswordHasher<Models.User> passwordHasher = new PasswordHasher<Models.User>();
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            
            return user;
        }
    }
}