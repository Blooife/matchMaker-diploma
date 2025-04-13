namespace User.BusinessLogic.Providers.Interfaces;

public interface IJwtTokenProvider
{
    string GenerateToken(DataAccess.Models.User applicationUser, IEnumerable<string> roles);
}