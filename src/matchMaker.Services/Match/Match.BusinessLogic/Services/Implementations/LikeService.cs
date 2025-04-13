using AutoMapper;
using Common.Exceptions;
using Match.BusinessLogic.DTOs.Like;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.BusinessLogic.Services.Implementations;

public class LikeService(IUnitOfWork _unitOfWork, IMapper _mapper) : ILikeService
{
    public async Task<LikeResponseDto> AddLikeAsync(AddLikeDto dto, CancellationToken cancellationToken)
    {
        var likeEntity = _mapper.Map<Like>(dto);

        var likerProfile = await _unitOfWork.Profiles.GetByIdAsync(likeEntity.ProfileId, cancellationToken);
        
        var likedProfile = await _unitOfWork.Profiles.GetByIdAsync(likeEntity.TargetProfileId, cancellationToken);

        if (likerProfile is null)
        {
            throw new NotFoundException(dto.ProfileId);
        }
        
        if (likedProfile is null)
        {
            throw new NotFoundException(dto.TargetProfileId);
        }
        
        var mutualLike = await _unitOfWork.Likes.CheckMutualLikeAsync(likeEntity, cancellationToken);
            
        if (mutualLike is not null)
        {
            var matchEntity = new MatchEntity()
            {
                FirstProfileId = likeEntity.ProfileId,
                SecondProfileId = likeEntity.TargetProfileId,
                Timestamp = DateTime.UtcNow
            };
            
            await _unitOfWork.Matches.CreateAsync(matchEntity, cancellationToken);
            
            await _unitOfWork.Likes.DeleteAsync(mutualLike, cancellationToken);
        }
        else
        {
            await _unitOfWork.Likes.CreateAsync(likeEntity, cancellationToken);
        }
        
        return _mapper.Map<LikeResponseDto>(likeEntity);
    }
}