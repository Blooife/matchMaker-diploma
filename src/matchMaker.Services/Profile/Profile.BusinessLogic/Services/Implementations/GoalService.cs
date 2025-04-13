using AutoMapper;
using Profile.BusinessLogic.DTOs.Goal.Response;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Interfaces;
using Profile.DataAccess.Providers.Interfaces.Repositories;

namespace Profile.BusinessLogic.Services.Implementations;

public class GoalService(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IGoalService
{
    private readonly string _cacheKeyPrefix = "goals";
    
    public async Task<IEnumerable<GoalResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<GoalResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var goals = await _unitOfWork.GoalRepository.GetAllAsync(cancellationToken);
        
        var mappedGoals = _mapper.Map<List<GoalResponseDto>>(goals);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedGoals, cancellationToken:cancellationToken);

        return mappedGoals;
    }
    
}