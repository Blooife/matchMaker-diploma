using Match.BusinessLogic.DTOs.Like;

namespace Match.BusinessLogic.Services.Interfaces;

public interface ILikeService
{
    Task<LikeResponseDto> AddLikeAsync(AddLikeDto dto, CancellationToken cancellationToken);
}