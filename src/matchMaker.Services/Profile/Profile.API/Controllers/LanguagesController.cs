using Common.Attributes;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BusinessLogic.DTOs.Language.Request;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class LanguagesController(ILanguageService _languageService, IProfileService _profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLanguages(CancellationToken cancellationToken)
    {
        var languages = await _languageService.GetAllAsync(cancellationToken);
        
        return Ok(languages);
    }

    [HttpPost("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<IActionResult> AddLanguageToProfile([FromBody] AddLanguageToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _profileService.AddLanguageToProfileAsync(dto.ProfileId, dto.LanguageId, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("profile")]
    [AuthorizeByOtherUser("ProfileId")]
    public async Task<IActionResult> RemoveLanguageFromProfile([FromBody] AddLanguageToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _profileService.RemoveLanguageFromProfileAsync(dto.ProfileId, dto.LanguageId, cancellationToken);
        
        return Ok(result);
    }
}