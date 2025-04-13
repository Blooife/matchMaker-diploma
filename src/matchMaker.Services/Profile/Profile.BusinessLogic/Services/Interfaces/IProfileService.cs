using Common.Dtos.Profile;
using Profile.BusinessLogic.DTOs.Interest.Response;
using Profile.BusinessLogic.DTOs.Language.Response;
using Profile.BusinessLogic.DTOs.Profile.Request;
using Profile.BusinessLogic.DTOs.Profile.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponseDto> CreateProfileAsync(CreateOrUpdateProfileDto requestDto, CancellationToken cancellationToken);
    Task<ProfileResponseDto> UpdateProfileAsync(long id, CreateOrUpdateProfileDto requestDto, CancellationToken cancellationToken);
    Task<ProfileResponseDto> GetProfileByIdAsync(long id, CancellationToken cancellationToken);
    Task<ProfileResponseDto> GetProfileByUserIdAsync(long id, CancellationToken cancellationToken);
    Task<List<InterestResponseDto>> AddInterestToProfileAsync(
        long profileId, long interestId, CancellationToken cancellationToken);

    Task<List<InterestResponseDto>> RemoveInterestFromProfileAsync(
        long profileId, long interestId, CancellationToken cancellationToken);
    Task<List<LanguageResponseDto>> AddLanguageToProfileAsync(
        long profileId, long languageId, CancellationToken cancellationToken);
    Task<List<LanguageResponseDto>> RemoveLanguageFromProfileAsync(
        long profileId, long languageId, CancellationToken cancellationToken);
    Task<ICollection<ProfileClientDto>> GetProfilesByIdsAsync(long[] profileIds, CancellationToken cancellationToken);
}