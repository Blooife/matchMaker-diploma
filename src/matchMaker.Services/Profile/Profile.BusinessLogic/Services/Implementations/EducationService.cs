using AutoMapper;
using Common.Exceptions;
using Profile.BusinessLogic.DTOs.Education.Request;
using Profile.BusinessLogic.DTOs.Education.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Models;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Profile.DataAccess.Specifications.ProfileSpecifications;

namespace Profile.BusinessLogic.Services.Implementations;

public class EducationService(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IEducationService
{
    private readonly string _cacheKeyPrefix = "educations";
    
    public async Task<IEnumerable<EducationResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<EducationResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var result = await _unitOfWork.EducationRepository.GetAllAsync(cancellationToken);
        
        var mappedEducations = _mapper.Map<List<EducationResponseDto>>(result);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedEducations, cancellationToken:cancellationToken);
        
        return mappedEducations;
    }
    
    public async Task<List<ProfileEducationResponseDto>> AddEducationToProfileAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == request.ProfileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException(request.EducationId);
        }
        
        var isProfileContainsEducation = profile.ContainsEducation(request.EducationId);

        if (isProfileContainsEducation)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsEducation);
        }

        var profileEducation = _mapper.Map<ProfileEducation>(request);
        await _unitOfWork.EducationRepository.AddEducationToProfileAsync(profile, profileEducation);
        await _unitOfWork.SaveAsync(cancellationToken);

        profileEducation.Education = education;
        
        return _mapper.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations);
    }
    
    public async Task<List<ProfileEducationResponseDto>> RemoveEducationFromProfileAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == request.ProfileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException(request.EducationId);
        }
        
        var isProfileContainsEducation = profile.ContainsEducation(request.EducationId);

        if (!isProfileContainsEducation)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        var profileEducation = profile.ProfileEducations.First(profileEducation => profileEducation.EducationId == request.EducationId);
        
        await _unitOfWork.EducationRepository.RemoveEducationFromProfileAsync(profile, profileEducation);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations);
    }
    
    public async Task<ProfileEducationResponseDto> UpdateProfileEducationAsync(
        AddOrRemoveProfileEducationDto request, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == request.ProfileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(request.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException(request.EducationId);
        }
        
        var isContains = profile.ContainsEducation(request.EducationId);

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        var profileEducation = profile.ProfileEducations.First(profileEducation => profileEducation.EducationId == request.EducationId);
        
        await _unitOfWork.EducationRepository.UpdateProfilesEducationAsync(profileEducation, request.Description);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<ProfileEducationResponseDto>(profileEducation);;
    }
}