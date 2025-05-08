using FluentValidation;
using User.BusinessLogic.DTOs.Request;

namespace User.API.Validators;

public class ModerateReportDtoValidator : AbstractValidator<ModerateReportDto>
{
    public ModerateReportDtoValidator()
    {
        RuleFor(x => x.BanUntil)
            .GreaterThan(DateTime.Now)
            .WithMessage("Дата блокировки должна быть в будущем");

        RuleFor(x => x.ModeratorComment)
            .NotNull()
            .WithMessage("Комментарий не должен быть пустым")
            .NotEmpty()
            .WithMessage("Комментарий не должен быть пустым")
            .Length(2, 1000)
            .WithMessage("Комментарий должен быть от 2 до 1000 символов");
    }
}