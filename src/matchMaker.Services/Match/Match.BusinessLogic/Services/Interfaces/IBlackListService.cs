using Match.BusinessLogic.DTOs.BlackList;

namespace Match.BusinessLogic.Services.Interfaces;

public interface IBlackListService
{
    Task AddToBlackListAsync(CreateBlackListDto dto, CancellationToken cancellationToken = default);
    Task<List<BlackListResponseDto>> GetBlackListForUserAsync(CancellationToken cancellationToken = default);
    Task RemoveFromBlackListAsync(RemoveFromBlackListDto dto, CancellationToken cancellationToken = default);
}