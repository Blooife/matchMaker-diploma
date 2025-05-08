using System.Security.Claims;
using Common.Constants;
using Microsoft.AspNetCore.Http;

namespace Common.Authorization.Context;

public class AuthenticationContext : IAuthenticationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimNames.UserIdClaimName)?.Value;

            return long.TryParse(userIdClaim, out var userId)
                ? userId : throw new FormatException("Идентификатор пользователя имеет неверный формат");
        }
    }
    
    public string IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    
    public bool IsUser => _httpContextAccessor.HttpContext?.User.Claims
        .Any(c => c is { Type: ClaimTypes.Role, Value: Roles.User }) ?? false;

    public bool IsModerator => _httpContextAccessor.HttpContext?.User.Claims
        .Any(c => c is { Type: ClaimTypes.Role, Value: Roles.Moderator }) ?? false;

    public bool IsAdmin => _httpContextAccessor.HttpContext?.User.Claims
        .Any(c => c is { Type: ClaimTypes.Role, Value: Roles.Admin }) ?? false;
}