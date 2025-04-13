using Common.Interfaces;

namespace Profile.DataAccess.Models;

public class User : BaseModel<long>, ISoftDeletable
{
    public string Email { get; set; }
    public DateTime? DeletedAt { get; set; }
    public UserProfile Profile { get; set; }
}