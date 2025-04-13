namespace User.BusinessLogic.DTOs.Response;

public class UserResponseDto
{
    public long Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; } 
}