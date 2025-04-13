using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Goal.Response;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class GoalsController(IGoalService _goalService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<GoalResponseDto>>> GetAllGoals(CancellationToken cancellationToken)
    {
        var goals = await _goalService.GetAllAsync(cancellationToken);
        
        return Ok(goals);
    }
}