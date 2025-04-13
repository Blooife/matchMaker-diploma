using AutoMapper;
using Profile.BusinessLogic.DTOs.Interest.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Services.Implementations;

public class InterestService(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IInterestService
{
    private readonly string _cacheKeyPrefix = "interests";
    
    public async Task<IEnumerable<InterestResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<InterestResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var interests = await _unitOfWork.InterestRepository.GetAllAsync(cancellationToken);
        
        var mappedInterests = _mapper.Map<List<InterestResponseDto>>(interests);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedInterests, cancellationToken:cancellationToken);
        
        return mappedInterests;
    }
}