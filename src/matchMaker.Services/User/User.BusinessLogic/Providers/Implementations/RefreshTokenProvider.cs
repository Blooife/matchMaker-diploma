using System.Security.Cryptography;
using User.BusinessLogic.Providers.Interfaces;

namespace User.BusinessLogic.Providers.Implementations;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    public string GenerateRefreshToken()
    {
        byte[] number = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(number);
            
            return Convert.ToBase64String(number);
        }
    }
}