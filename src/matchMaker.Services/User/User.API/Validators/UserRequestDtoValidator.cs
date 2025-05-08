using FluentValidation;
using User.BusinessLogic.DTOs.Request;

namespace User.API.Validators;

public class UserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    public UserRequestDtoValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Адрес почты обязателен для заполнения")
            .MaximumLength(100).WithMessage("Email length must be less than 100")
            .EmailAddress().WithMessage("Невалидный адрес почты");
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен для заполнения")
            .MinimumLength(6).WithMessage("Минимальная длина пароля 6 символов");
    }
}