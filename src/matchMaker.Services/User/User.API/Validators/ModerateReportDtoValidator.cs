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
    }
}