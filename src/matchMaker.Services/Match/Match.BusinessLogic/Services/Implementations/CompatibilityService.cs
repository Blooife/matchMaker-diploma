using System.Net.Http.Json;
using Match.BusinessLogic.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace Match.BusinessLogic.Services.Implementations;

public class CompatibilityService : ICompatibilityService
{
    private readonly HttpClient _httpClient;

    public CompatibilityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetBirthCompatibilityAsync(DateTime birthDate1, DateTime birthDate2)
    {
        var requestData = new object[] {
            new[] { birthDate1.Day.ToString("D2"), birthDate1.Month.ToString("D2"), birthDate1.Year.ToString() },
            new[] { birthDate2.Day.ToString("D2"), birthDate2.Month.ToString("D2"), birthDate2.Year.ToString() }
        };

        var response = await _httpClient.PostAsJsonAsync("birth-compatibility", requestData);

        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(jsonString);

        // Можно обработать json, чтобы вернуть только нужные поля

        return jsonString;
    }
}