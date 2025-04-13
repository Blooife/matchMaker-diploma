using Profile.BusinessLogic.DTOs.Country.Response;

namespace Profile.BusinessLogic.DTOs.City.Response;

public class CityWithCountryResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public CountryResponseDto Country { get; set; }
}