namespace User.BusinessLogic.DTOs.Request;

public class ChangePasswordRequestDto
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
