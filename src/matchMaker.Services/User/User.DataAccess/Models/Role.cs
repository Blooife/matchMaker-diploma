using Microsoft.AspNetCore.Identity;

namespace User.DataAccess.Models;

public class Role : IdentityRole<long>
{
    public IEnumerable<User>? Users { get; set; } = [];
}