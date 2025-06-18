using Newtonsoft.Json.Linq;

namespace Match.BusinessLogic.Services.Interfaces;

public interface ICompatibilityService
{
    Task<string> GetBirthCompatibilityAsync(DateTime birthDate1, DateTime birthDate2);
}