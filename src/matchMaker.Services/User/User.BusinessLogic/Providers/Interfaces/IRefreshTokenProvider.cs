namespace User.BusinessLogic.Providers.Interfaces;

public interface IRefreshTokenProvider
{
    string GenerateRefreshToken();
}