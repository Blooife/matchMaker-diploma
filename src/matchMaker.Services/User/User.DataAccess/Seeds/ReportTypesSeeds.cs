using Common.Database;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Contexts;
using User.DataAccess.Models;

namespace User.DataAccess.Seeds;

public class ReportTypesSeeds(UserContext _dbContext) : ISeedEntitiesProvider<UserContext>
{
    public async Task SeedAsync()
    {
        foreach (var reportType in GetReportTypes())
        {
            await CreateIfNotExists(reportType);
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task CreateIfNotExists(ReportType reportType)
    {
        var hasAny = await _dbContext.Set<ReportType>().AnyAsync(x => x.Name == reportType.Name);
        if (!hasAny)
        {
            await _dbContext.Set<ReportType>().AddAsync(reportType);
        }
    }

    private IEnumerable<ReportType> GetReportTypes()
    {
        yield return Create("Пропаганда наркотиков", "DRUG_PROPAGANDA");
        yield return Create("Сексуальный контент", "SEXUAL_CONTENT");
        yield return Create("Ненавистнические высказывания", "HATE_SPEECH");
        yield return Create("Домогательства", "HARASSMENT");
        yield return Create("Спам", "SPAM");
        yield return Create("Другое", "OTHER");

        yield break;

        ReportType Create(string name, string normalizedName)
        {
            return new ReportType
            {
                Name = name,
            };
        }
    }
}