using AutoMapper;
using Common.Exceptions;
using MessageQueue;
using MessageQueue.Messages.Profile;
using Profile.BusinessLogic.DTOs.Image.Request;
using Profile.BusinessLogic.DTOs.Image.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Models;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Services.Implementations;

public class ImageService
    (IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService, ICommunicationBus _communicationBus) : IImageService
{
    private readonly string[] _allowedExtensions = [ ".jpg", ".jpeg", ".png" ];
    
    public async Task<IEnumerable<ImageResponseDto>> AddImageToProfileAsync(AddImageDto request, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == request.ProfileId, cancellationToken);

        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(request.ProfileId);
        }
        
        var file = request.file;
        var fileExtension = Path.GetExtension(file.FileName); 

        if (!_allowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            throw new ImageUploadException("Wrong extension");
        }

        var objectName = $"{request.ProfileId}/{file.FileName}";
        
        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;
        await _minioService.UploadFileAsync(objectName, file);

        bool isMain = profile.Images.Count == 0;
        
        var imageEntity = new Image
        {
            ProfileId = request.ProfileId,
            ImageUrl = $"http://{_minioService.Endpoint}/{_minioService.BucketName}/{objectName}",
            IsMainImage = isMain,
            UploadTimestamp = DateTime.UtcNow
        };

        var result = await _unitOfWork.ImageRepository.AddImageToProfileAsync(imageEntity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        profile.Images.Add(result);
        var sortedImages = profile.Images
            .OrderByDescending(i => i.IsMainImage)
            .ThenByDescending(i => i.UploadTimestamp)
            .ToList();
        profile.Images = sortedImages;

        if (isMain)
        {
            var profileUpdatedMessage = _mapper.Map<ProfileUpdatedEventMessage>(profile);
            await _communicationBus.PublishAsync(profileUpdatedMessage, cancellationToken);
        }
        
        return _mapper.Map<IEnumerable<ImageResponseDto>>(sortedImages);
    }
    
    public async Task<IEnumerable<ImageResponseDto>> ChangeMainImageAsync(ChangeMainImageDto request, CancellationToken cancellationToken)
    {
        var profileResponseDto = 
            await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == request.ProfileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(request.ProfileId);
        }
        
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.ImageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException(request.ImageId);
        }
        
        var notMainImage = profile.Images.First(p => p.IsMainImage);
        var mainImage = profile.Images.First(p => p.Id == image.Id);
        await _unitOfWork.ImageRepository.UpdateIsMainImageAsync(notMainImage.Id, false, cancellationToken);
        await _unitOfWork.ImageRepository.UpdateIsMainImageAsync(mainImage.Id, true, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var sortedImages = profile.Images
            .OrderByDescending(i => i.IsMainImage)
            .ThenByDescending(i => i.UploadTimestamp)
            .ToList();
        profile.Images = sortedImages;
        
        var profileUpdatedMessage = _mapper.Map<ProfileUpdatedEventMessage>(profile);
        await _communicationBus.PublishAsync(profileUpdatedMessage, cancellationToken);
        
        return  _mapper.Map<IEnumerable<ImageResponseDto>>(profile.Images);
    }
    
    public async Task<ImageResponseDto> RemoveImageAsync(
        long profileId, long imageId, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == profileId, cancellationToken);

        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }
        
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(imageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException(imageId);
        }
        
        await _unitOfWork.ImageRepository.RemoveImageFromProfileAsync(image);
        profile.Images.Remove(image);
        
        if (profile.Images.Count > 0 && !profile.Images[0].IsMainImage)
        {
            await _unitOfWork.ImageRepository.UpdateIsMainImageAsync(profile.Images[0].Id, true, cancellationToken);
            
            var profileUpdatedMessage = _mapper.Map<ProfileUpdatedEventMessage>(profile);
            await _communicationBus.PublishAsync(profileUpdatedMessage, cancellationToken);
        }
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _minioService.DeleteFileAsync(image.ImageUrl);
        
        return  _mapper.Map<ImageResponseDto>(image);
    }
}