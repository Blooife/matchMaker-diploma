using Profile.BusinessLogic.DTOs.Education.Request;
using Profile.BusinessLogic.DTOs.Education.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface IEducationService
{
    Task<IEnumerable<EducationResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<ProfileEducationResponseDto>> AddEducationToProfileAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken);
    Task<List<ProfileEducationResponseDto>> RemoveEducationFromProfileAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken);
    Task<ProfileEducationResponseDto> UpdateProfileEducationAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken);
}