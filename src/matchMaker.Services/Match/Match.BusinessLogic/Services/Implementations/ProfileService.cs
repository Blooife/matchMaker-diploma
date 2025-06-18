using AutoMapper;
using Common.Authorization.Context;
using Common.Dtos.Profile;
using Common.Exceptions;
using Match.BusinessLogic.DTOs.Profile;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver.GeoJsonObjectModel;
using Profile.Client;

namespace Match.BusinessLogic.Services.Implementations;

public class ProfileService(
    IUnitOfWork _unitOfWork,
    IProfileClient _profileClient,
    IMapper _mapper,
    IAuthenticationContext _authenticationContext,
    IHybridScoringService _hybridScoringService) : IProfileService
{
    public async Task<List<ProfileResponseDto>> GetRecommendationsAsync(
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var profileId = _authenticationContext.UserId;
        var userProfile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (userProfile is null)
        {
            throw new NotFoundException("Профиль");
        }

        var likedProfiles = await _unitOfWork.Likes
            .GetAsync(like => like.ProfileId == profileId, cancellationToken);
        var likedProfilesIds = likedProfiles.Select(l => l.TargetProfileId);
        
        var matchedProfiles = await _unitOfWork.Matches
            .GetAsync(match => match.FirstProfileId == profileId || match.SecondProfileId == profileId, cancellationToken);
        var matchedProfilesIds = matchedProfiles.Select(match => match.FirstProfileId == profileId ? match.SecondProfileId : match.FirstProfileId);

        var excludedProfileIds = likedProfilesIds.Concat(matchedProfilesIds).Distinct().ToList();

        var ids =
            await _unitOfWork.Profiles.GetRecsAsync(excludedProfileIds, userProfile, cancellationToken);

        var candidateProfiles = await _profileClient.GetProfilesByIdsAsync(ids.ToArray());
        var userClientProfile = (await _profileClient.GetProfilesByIdsAsync([profileId])).FirstOrDefault();
        
        var ranked = await _hybridScoringService.RankProfilesAsync(userClientProfile, candidateProfiles.ToList(), cancellationToken);

        return _mapper.Map<List<ProfileResponseDto>>(ranked);
    }
    
    public async Task UpdateLocationAsync(UpdateLocationDto dto, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Профиль");
        }

        if (dto.Longitude is not null && dto.Latitude is not null)
        {
            profile.Location =
                new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(dto.Latitude.Value, dto.Longitude.Value));
        }
        else
        {
            profile.Location = null;
        }
        
        await _unitOfWork.Profiles.UpdateAsync(profile, cancellationToken);
    }
    
    public async Task<ProfileResponseDto> GetByIdAsync(long profileId, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Профиль");
        }

        var res = new ProfileResponseDto()
        {
            Name = profile.Name,
            LastName = profile.LastName,
        };

        return res;
    }
}