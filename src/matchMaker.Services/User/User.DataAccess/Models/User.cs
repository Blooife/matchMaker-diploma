using Microsoft.AspNetCore.Identity;

namespace User.DataAccess.Models;

public class User : IdentityUser<long>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public List<Role> Roles { get; set; } = [];
}