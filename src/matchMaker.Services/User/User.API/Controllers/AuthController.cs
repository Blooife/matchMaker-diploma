using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using User.BusinessLogic.DTOs.Request;
using User.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using User.BusinessLogic.DTOs.Response;

namespace User.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register([FromBody] UserRequestDto model)
    {
        var response = await _authService.RegisterAsync(model);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(model, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshRequestDto refreshToken, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshToken.refreshToken, cancellationToken);

        return Ok(response);
    }
    [HttpPost("login/google")]
    public async Task<ActionResult<LoginResponseDto>> LoginWithGoogle([FromBody] ExternalLoginRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginWithGoogleAsync(model);
        
        return Ok(response);
    }
    
    [HttpPatch("password")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto, CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(dto, cancellationToken);
        
        return NoContent();
    }
}