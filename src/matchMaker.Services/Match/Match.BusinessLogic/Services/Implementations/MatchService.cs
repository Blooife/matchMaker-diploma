using AutoMapper;
using Common.Exceptions;
using Common.Models;
using Match.BusinessLogic.DTOs.Profile;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Providers.Interfaces;
using Profile.Client;

namespace Match.BusinessLogic.Services.Implementations;

public class MatchService(IUnitOfWork _unitOfWork, IProfileClient _profileClient, IMapper _mapper) : IMatchService
{
    public async Task<PagedList<ProfileResponseDto>> GetMatchesByProfileIdAsync(
        long profileId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException(profileId);
        }

        var (matches, count) = await _unitOfWork.Matches.GetPagedAsync(profileId, pageNumber, pageSize, cancellationToken);
        
        var filteredIds = matches
            .SelectMany(m => new[] { m.FirstProfileId, m.SecondProfileId })  
            .Where(id => id != profileId)  
            .Distinct()  
            .ToArray();
        
        var profiles = await _profileClient.GetProfilesByIdsAsync(filteredIds);
        
        return new PagedList<ProfileResponseDto>(_mapper.Map<List<ProfileResponseDto>>(profiles), count, pageNumber, pageSize);
    }
}