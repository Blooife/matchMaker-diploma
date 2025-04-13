using Common.Models;
using Match.BusinessLogic.DTOs.Profile;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IMatchService
{
    Task<PagedList<ProfileResponseDto>> GetMatchesByProfileIdAsync(
        long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken);
}