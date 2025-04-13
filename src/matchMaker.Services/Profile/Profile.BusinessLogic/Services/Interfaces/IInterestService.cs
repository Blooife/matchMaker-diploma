using Profile.BusinessLogic.DTOs.Interest.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface IInterestService
{
    Task<IEnumerable<InterestResponseDto>> GetAllAsync(CancellationToken cancellationToken);
}