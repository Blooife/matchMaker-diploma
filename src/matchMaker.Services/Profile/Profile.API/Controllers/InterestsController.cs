using Common.Attributes;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Interest.Request;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class InterestsController(IInterestService _interestService, IProfileService _profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllInterests(CancellationToken cancellationToken)
    {
        var interests = await _interestService.GetAllAsync(cancellationToken);
        
        return Ok(interests);
    }

    [HttpPost("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<IActionResult> AddInterestToProfile([FromBody] AddInterestToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _profileService.AddInterestToProfileAsync(dto.ProfileId, dto.InterestId, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<IActionResult> RemoveInterestFromProfile([FromBody] AddInterestToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _profileService.RemoveInterestFromProfileAsync(dto.ProfileId, dto.InterestId, cancellationToken);
        
        return Ok(result);
    }
}