using Common.Database;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Models;

namespace Profile.DataAccess.Seeds;

public class GoalsSeeds(ProfileDbContext _dbContext) : ISeedEntitiesProvider<ProfileDbContext>
{
    public Task SeedAsync()
    {
        foreach (var goal in GetGoals())
        {
            CreateIfNotExists(goal);
        }

        _dbContext.SaveChanges();
        return Task.CompletedTask;
    }
    
    private void CreateIfNotExists(Goal goal)
    {
        var hasAny = _dbContext.Set<Goal>().Any(x => x.Name == goal.Name);
        if (!hasAny)
        {
            _dbContext.Set<Goal>().Add(goal);
        }
    }
    
    private IEnumerable<Goal> GetGoals()
    {
        yield return Create("Дружба");
        yield return Create("Общение");
        yield return Create("Отношения");
        yield return Create("Серьёзные отношения");
        yield break;

        Goal Create(string name)
        {
            return new Goal()
            {
                Name = name,
            };
        }
    }
}