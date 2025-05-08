using FluentValidation;
using User.BusinessLogic.DTOs.Request;

namespace User.API.Validators;

public class CreateUserReportDtoValidator : AbstractValidator<CreateUserReportDto>
{
    public CreateUserReportDtoValidator()
    {
        RuleFor(x => x.Reason)
            .NotNull()
            .WithMessage("Причина не должна быть пустой")
            .NotEmpty()
            .WithMessage("Причина не должна быть пустой")
            .Length(2, 1000)
            .WithMessage("Причина должна быть от 2 до 1000 символов");
    }
}