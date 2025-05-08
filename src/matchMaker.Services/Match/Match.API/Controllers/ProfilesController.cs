using Common.Attributes;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Match.BusinessLogic.DTOs.Profile;
using Match.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Match.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class ProfilesController(IProfileService _profileService) : ControllerBase
{
    [HttpGet("recommendations")]
    public async Task<ActionResult<ICollection<ProfileResponseDto>>> GetPagedRecommendations(
         CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var profiles = await _profileService.GetRecommendationsAsync(pageNumber, pageSize, cancellationToken);

        return Ok(profiles);
    }
    
    [HttpPatch("location")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<ActionResult> UpdateLocation([FromBody]UpdateLocationDto dto, CancellationToken cancellationToken)
    {
        await _profileService.UpdateLocationAsync(dto, cancellationToken);

        return Ok();
    }
}