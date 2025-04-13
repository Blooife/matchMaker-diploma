using Profile.BusinessLogic.DTOs.City.Response;
using Profile.BusinessLogic.DTOs.Country.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<CountryResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<CityResponseDto>> GetAllCitiesByCountryId(long countryId, CancellationToken cancellationToken);
}