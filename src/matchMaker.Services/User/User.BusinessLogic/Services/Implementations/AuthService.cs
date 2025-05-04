using User.BusinessLogic.DTOs.Response;
using AutoMapper;
using Common.Constants;
using Common.Exceptions;
using MessageQueue;
using MessageQueue.Messages.User;
using Microsoft.Extensions.Logging;
using User.BusinessLogic.DTOs.Request;
using User.BusinessLogic.Providers.Interfaces;
using User.BusinessLogic.Services.Interfaces;
using User.DataAccess.Providers.Interfaces;

namespace User.BusinessLogic.Services.Implementations;

public class AuthService(
    IUserProvider _userRepository,
    IMapper _mapper,
    ILogger<AuthService> _logger,
    IJwtTokenProvider _jwtTokenProvider,
    IRefreshTokenProvider _refreshTokenProvider,
    ICommunicationBus _communicationBus) : IAuthService
{
    private const int RefreshTokenExpiresInDays = 7;
    
    public async Task<UserResponseDto> RegisterAsync(UserRequestDto registrationRequestDto)
    {
        var user = _mapper.Map<DataAccess.Models.User>(registrationRequestDto);
        
        var result = await _userRepository.RegisterAsync(user, registrationRequestDto.Password);
        
        if (!result.Succeeded)
        {
            _logger.LogError("User registration failed with errors: {errors}", result.Errors.Select(e => e.Description).ToArray());
            throw new RegisterException(result.Errors.First().Description);
        }
        
        await _userRepository.AddToRoleAsync(user, Roles.User);
        
        await _communicationBus.PublishAsync(new UserCreatedEventMessage()
        {
            Id = user.Id,
            Email = user.Email,
        });
        
        return new  UserResponseDto()
        {
            Id = user.Id,
            Email =  user.Email,
            Roles = [Roles.User],
        };
    }

    public async Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email, cancellationToken);
            
        if (user is null)
        {
            _logger.LogError("Login failed: user with email = {email} was not found", loginRequestDto.Email);
            throw new NotFoundException(loginRequestDto.Email);
        }

        if (user.IsBanned && (user.BannedUntil == null || user.BannedUntil > DateTime.UtcNow))
        {
            var untilText = user.BannedUntil == null
                ? "навсегда"
                : $"до {user.BannedUntil.Value.ToLocalTime():g}";

            throw new LoginException($"Вы были заблокированы {untilText}.");
        }

        var isValid = await _userRepository.CheckPasswordAsync(user, loginRequestDto.Password);

        if (isValid == false)
        {
            _logger.LogError("Login failed: Incorrect password for user with email = {email}", loginRequestDto.Email);
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
        
        return await GetLoginResponseDtoWithGeneratedTokens(user);
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);
            
        if (user is null)
        {
            _logger.LogError("Refresh failed: user with refresh token = {token} was not found", refreshToken);
            throw new NotFoundException(refreshToken);
        }
            
        if(user.RefreshTokenExpiredAt < DateTime.Now)
        {
            _logger.LogError("Refresh failed: refresh token expired at {at}", user.RefreshTokenExpiredAt);
            throw new LoginException("Токен просрочен");
        }

        return await GetLoginResponseDtoWithGeneratedTokens(user);
    }

    private async Task<LoginResponseDto> GetLoginResponseDtoWithGeneratedTokens(DataAccess.Models.User user)
    {
        var roles = await _userRepository.GetRolesAsync(user);
            
        user.RefreshToken = _refreshTokenProvider.GenerateRefreshToken();
        user.RefreshTokenExpiredAt = DateTime.UtcNow.AddDays(RefreshTokenExpiresInDays);

        await _userRepository.UpdateUserAsync(user);
            
        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.JwtToken = _jwtTokenProvider.GenerateToken(user, roles);
        
        return loginResponseDto;
    }
}