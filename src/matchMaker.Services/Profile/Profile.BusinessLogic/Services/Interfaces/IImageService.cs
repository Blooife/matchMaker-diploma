using Profile.BusinessLogic.DTOs.Image.Request;
using Profile.BusinessLogic.DTOs.Image.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface IImageService
{
    Task<IEnumerable<ImageResponseDto>> AddImageToProfileAsync(AddImageDto request, CancellationToken cancellationToken);
    Task<IEnumerable<ImageResponseDto>> ChangeMainImageAsync(ChangeMainImageDto request, CancellationToken cancellationToken);
    Task<ImageResponseDto> RemoveImageAsync(
        long profileId, long imageId, CancellationToken cancellationToken);
}