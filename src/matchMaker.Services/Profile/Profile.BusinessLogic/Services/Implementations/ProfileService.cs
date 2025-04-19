using AutoMapper;
using Common.Dtos.Profile;
using Common.Exceptions;
using Common.Exceptions.Messages;
using MessageQueue;
using MessageQueue.Messages.Profile;
using Profile.BusinessLogic.DTOs.Profile.Request;
using Profile.BusinessLogic.DTOs.Profile.Response;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Models;
using Profile.DataAccess.Providers.Interfaces.Repositories;
using Profile.DataAccess.Specifications.ProfileSpecifications;
using InterestResponseDto = Profile.BusinessLogic.DTOs.Interest.Response.InterestResponseDto;
using LanguageResponseDto = Profile.BusinessLogic.DTOs.Language.Response.LanguageResponseDto;

namespace Profile.BusinessLogic.Services.Implementations;

public class ProfileService(IUnitOfWork _unitOfWork, IMapper _mapper, ICommunicationBus _communicationBus) : IProfileService
{
    public async Task<ProfileResponseDto> CreateProfileAsync(CreateOrUpdateProfileDto requestDto, CancellationToken cancellationToken)
    {
        var profile = _mapper.Map<UserProfile>(requestDto);
        profile.Id = profile.UserId;
        var result = await _unitOfWork.ProfileRepository.CreateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var fullProfile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == profile.Id, cancellationToken);
        var mappedProfile = _mapper.Map<ProfileResponseDto>(fullProfile);

        var profileCreatedMessage = _mapper.Map<ProfileCreatedEventMessage>(fullProfile);
        await _communicationBus.PublishAsync(profileCreatedMessage, cancellationToken);
        
        return mappedProfile;
    }
    
    public async Task<ProfileResponseDto> UpdateProfileAsync(long id, CreateOrUpdateProfileDto requestDto, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(id, cancellationToken);
        
        if (findRes is null)
        {
            throw new NotFoundException(id);
        }
        
        var profile = _mapper.Map<UserProfile>(requestDto);
        profile.Id = id;
        var result = await _unitOfWork.ProfileRepository.UpdateProfileAsync(profile);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var fullProfile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == profile.Id, cancellationToken);
        var mappedProfile = _mapper.Map<ProfileResponseDto>(fullProfile);
        
        var profileUpdatedMessage = _mapper.Map<ProfileUpdatedEventMessage>(fullProfile);
        await _communicationBus.PublishAsync(profileUpdatedMessage, cancellationToken);
        
        return mappedProfile;
    }
    
    public async Task<ProfileResponseDto> GetProfileByIdAsync(long id, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException(id);
        }
        
        var mappedProfile = _mapper.Map<ProfileResponseDto>(profile);
        
        return mappedProfile;
    }
    
    public async Task<ProfileResponseDto> GetProfileByUserIdAsync(long id , CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(p=>p.UserId == id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException(id);
        }
        
        var mappedProfile = _mapper.Map<ProfileResponseDto>(profile);
        
        return mappedProfile;
    }
    
    public async Task<List<LanguageResponseDto>> AddLanguageToProfileAsync(
        long profileId, long languageId, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == profileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }

        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(languageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException(languageId);
        }

        var isProfileContainsLanguage = profile.ContainsLanguage(languageId);

        if (isProfileContainsLanguage)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsLanguage);
        }
        
        await _unitOfWork.LanguageRepository.AddLanguageToProfileAsync(profile, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<List<LanguageResponseDto>>(profile.Languages);
    }
    
    public async Task<List<LanguageResponseDto>> RemoveLanguageFromProfileAsync(
        long profileId, long languageId, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == profileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(languageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException(languageId);
        }

        var isProfileContainsLanguage = profile.ContainsLanguage(languageId);

        if (!isProfileContainsLanguage)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsLanguage);
        }

        var languageToRemove = profile.Languages.First(l=>l.Id == languageId);
        await _unitOfWork.LanguageRepository.RemoveLanguageFromProfileAsync(profile, languageToRemove);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<List<LanguageResponseDto>>(profile.Languages);
    }
    
    public async Task<List<InterestResponseDto>> AddInterestToProfileAsync(
        long profileId, long interestId, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == profileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);

        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(interestId, cancellationToken);
        
        if (interest is null)
        {
            throw new NotFoundException(interestId);
        }

        var isProfileContainsInterest = profile.ContainsInterest(interestId);

        if (isProfileContainsInterest)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsInterest);
        }
        
        var lessThan = profile.InterestsLessThan(6);

        if (!lessThan)
        {
            throw new Exception("You exceeded maximum amount of interests");
        }
        
        await _unitOfWork.InterestRepository.AddInterestToProfileAsync(profile, interest);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<List<InterestResponseDto>>(profile.Interests);
    }
    
    public async Task<List<InterestResponseDto>> RemoveInterestFromProfileAsync(
        long profileId, long interestId, CancellationToken cancellationToken)
    {
        var profileResponseDto = await _unitOfWork.ProfileRepository
            .GetAllProfileInfoAsync(userProfile => userProfile.Id == profileId, cancellationToken);
        
        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(interestId, cancellationToken);
        
        if (interest is null)
        {
            throw new NotFoundException(interestId);
        }

        var isProfileContainsInterest = profile.ContainsInterest(interestId);

        if (!isProfileContainsInterest)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsInterest);
        }
        
        var interestToRemove = profile.Interests.First(i=>i.Id == interestId);
        await _unitOfWork.InterestRepository.RemoveInterestFromProfileAsync(profile, interestToRemove);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<List<InterestResponseDto>>(profile.Interests);
    }

    public async Task<ICollection<ProfileClientDto>> GetProfilesByIdsAsync(long[] profileIds, CancellationToken cancellationToken)
    {
        var profiles = await _unitOfWork.ProfileRepository.GetAllProfileInfoByIdsAsync(profileIds);
        
        return _mapper.Map<List<ProfileClientDto>>(profiles);
    }
}