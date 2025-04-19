using Microsoft.EntityFrameworkCore;
using Profile.DataAccess.Models;
using Profile.DataAccess.Contexts;
using Profile.DataAccess.Implementations.BaseRepositories;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.DataAccess.Providers.Implementations.Repositories;

public class ImageRepository(ProfileDbContext _dbContext)
    : GenericRepository<Image, long>(_dbContext), IImageRepository
{
    public async Task<Image> AddImageToProfileAsync(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
        
        return image;
    }
    
    public async Task RemoveImageFromProfileAsync(Image image)
    {
        _dbContext.Images.Remove(image);
    }
    
    public async Task UpdateImageAsync(Image image)
    {
        _dbContext.Images.Update(image);
    }
    
    public async Task UpdateIsMainImageAsync(long imageId, bool isMainImage, CancellationToken cancellationToken)
    {
        var image = await _dbContext.Images.AsNoTracking().FirstOrDefaultAsync(x => x.Id == imageId, cancellationToken);
        image.IsMainImage = isMainImage;
        _dbContext.Images.Update(image);
    }
}