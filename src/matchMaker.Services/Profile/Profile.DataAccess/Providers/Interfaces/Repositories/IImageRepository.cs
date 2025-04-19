using Profile.DataAccess.Models;
using Profile.DataAccess.Interfaces.BaseRepositories;

namespace Profile.DataAccess.Providers.Interfaces.Repositories;

public interface IImageRepository : IGenericRepository<Image, long>
{
    Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken);
    Task RemoveImageFromProfileAsync(Image image);
    Task UpdateImageAsync(Image image);
    Task UpdateIsMainImageAsync(long imageId, bool isMainImage, CancellationToken cancellationToken);
}