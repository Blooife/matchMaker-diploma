using Common.Attributes;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Education.Request;
using Profile.BusinessLogic.DTOs.Education.Response;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class EducationsController(IEducationService _educationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ICollection<EducationResponseDto>>> GetAllEducations(CancellationToken cancellationToken)
    {
        var education = await _educationService.GetAllAsync(cancellationToken);
        
        return Ok(education);
    }
    
    [HttpPut("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<ActionResult<ICollection<ProfileEducationResponseDto>>> UpdateProfileEducation(
        [FromBody] AddOrRemoveProfileEducationDto dto, CancellationToken cancellationToken)
    {
        var result = await _educationService.UpdateProfileEducationAsync(dto, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<ActionResult<ICollection<ProfileEducationResponseDto>>> AddEducationToProfile(
        [FromBody] AddOrRemoveProfileEducationDto dto, CancellationToken cancellationToken)
    {
        var result = await _educationService.AddEducationToProfileAsync(dto, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<ActionResult<ICollection<ProfileEducationResponseDto>>> RemoveEducationFromProfile(
        [FromBody] AddOrRemoveProfileEducationDto dto, CancellationToken cancellationToken)
    {
        var result = await _educationService.RemoveEducationFromProfileAsync(dto, cancellationToken);
        
        return Ok(result);
    }
}