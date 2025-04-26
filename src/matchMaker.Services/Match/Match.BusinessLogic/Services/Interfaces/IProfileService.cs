using Match.BusinessLogic.DTOs.Profile;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IProfileService
{
    Task<List<ProfileResponseDto>> GetRecommendationsAsync(
        long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task UpdateLocationAsync(UpdateLocationDto dto, CancellationToken cancellationToken);
    Task<ProfileResponseDto> GetByIdAsync(long profileId, CancellationToken cancellationToken);
}