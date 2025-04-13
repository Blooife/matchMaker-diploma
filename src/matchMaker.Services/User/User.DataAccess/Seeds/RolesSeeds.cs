using Common.Database;
using User.DataAccess.Contexts;
using User.DataAccess.Models;

namespace User.DataAccess.Seeds;

public class RolesSeeds(UserContext _dbContext) : ISeedEntitiesProvider<UserContext>
{
    public Task SeedAsync()
    {
        foreach (var role in GetRoles())
        {
            CreateIfNotExists(role);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Role role)
    {
        var hasAny = _dbContext.Set<Role>().Any(x => x.Name == role.Name);
        if (!hasAny)
        {
            _dbContext.Set<Role>().Add(role);
        }
    }
    
    private IEnumerable<Role> GetRoles()
    {
        yield return Create("User", "USER");
        yield return Create("Admin", "ADMIN");
        yield return Create("Moderator", "MODERATOR");
        yield break;

        Role Create(string name, string normalizedName)
        {
            return new Role
            {
                Name = name,
                NormalizedName = normalizedName,
            };
        }
    }
}