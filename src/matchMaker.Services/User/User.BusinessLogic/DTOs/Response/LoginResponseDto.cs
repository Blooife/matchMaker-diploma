namespace User.BusinessLogic.DTOs.Response;

public class LoginResponseDto
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public string JwtToken { get; set; }
}