using Common.Models;
using Match.BusinessLogic.DTOs.Profile;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IMatchService
{
    Task<PagedList<ProfileResponseDto>> GetMatchesByProfileIdAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken);
}