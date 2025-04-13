using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class EducationsSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        foreach (var education in GetEducations())
        {
            CreateIfNotExists(education);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Education education)
    {
        var hasAny = _dbContext.Set<Education>().Any(x => x.Name == education.Name);
        if (!hasAny)
        {
            _dbContext.Set<Education>().Add(education);
        }
    }
    
    private IEnumerable<Education> GetEducations()
    {
        yield return Create("Высшее");
        yield return Create("Среднее специальное");
        yield return Create("Среднее");
        yield return Create("Неоконченное высшее");
        yield break;

        Education Create(string name)
        {
            return new Education()
            {
                Name = name,
            };
        }
    }
}