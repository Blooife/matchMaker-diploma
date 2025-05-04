using Common.Constants;
using Common.Dtos.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Profile.Request;
using Profile.BusinessLogic.DTOs.Profile.Response;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/profiles")]
[ApiController]
public class ProfilesController(IProfileService _profileService) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Roles = $"{Roles.User}, {Roles.Moderator}, {Roles.Admin}")]
    public async Task<ActionResult<ProfileResponseDto>> GetProfileById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var profile = await _profileService.GetProfileByIdAsync(id, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpGet("user/{id}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<ActionResult<ProfileResponseDto>> GetProfileByUserId([FromRoute] long id, CancellationToken cancellationToken)
    {
        var profile = await _profileService.GetProfileByUserIdAsync(id, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpPost]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<ActionResult<ProfileResponseDto>> CreateProfile([FromBody] CreateOrUpdateProfileDto dto, CancellationToken cancellationToken)
    {
        var profile = await _profileService.CreateProfileAsync(dto, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<ActionResult<ProfileResponseDto>> UpdateProfile([FromRoute] long id, [FromBody] CreateOrUpdateProfileDto dto, CancellationToken cancellationToken)
    {
        var profile = await _profileService.UpdateProfileAsync(id, dto, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpPost("by-ids/get")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<ActionResult<ICollection<ProfileClientDto>>> GetProfilesByIds([FromBody] long[] profileIds, CancellationToken cancellationToken)
    {
        var profiles = await _profileService.GetProfilesByIdsAsync(profileIds, cancellationToken);
        
        return Ok(profiles);
    }
}