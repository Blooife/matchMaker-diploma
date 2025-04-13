using User.BusinessLogic.DTOs.Response;
using Common.Models;
using User.BusinessLogic.DTOs.Request;

namespace User.BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(UserRequestDto registrationRequestDto);
    Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}