using FluentValidation;
using User.BusinessLogic.DTOs.Request;

namespace User.API.Validators;

public class UserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    public UserRequestDtoValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(100).WithMessage("Email length must be less than 100")
            .EmailAddress().WithMessage("Invalid email");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Minimum length of password is 6");
    }
}