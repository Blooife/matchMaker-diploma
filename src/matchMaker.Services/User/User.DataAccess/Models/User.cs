using Microsoft.AspNetCore.Identity;

namespace User.DataAccess.Models;

public class User : IdentityUser<long>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsBanned { get; set; } = false;
    public DateTime? BannedUntil { get; set; } 
    public List<Role> Roles { get; set; } = [];
    
    public ICollection<UserReport> ReportsMade { get; set; } = [];
    public ICollection<UserReport> ReportsReceived { get; set; } = [];
    public ICollection<UserReport> ReportsReviewed { get; set; } = [];
}