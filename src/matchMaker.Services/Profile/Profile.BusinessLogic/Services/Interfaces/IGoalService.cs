using Profile.BusinessLogic.DTOs.Goal.Response;

namespace Profile.BusinessLogic.Services.Interfaces;

public interface IGoalService
{
    Task<IEnumerable<GoalResponseDto>> GetAllAsync(CancellationToken cancellationToken);
}