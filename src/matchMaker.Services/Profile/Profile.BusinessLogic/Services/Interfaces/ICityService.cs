using Profile.BusinessLogic.DTOs.City.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface ICityService
{
    Task<IEnumerable<CityResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<CityResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken);
}