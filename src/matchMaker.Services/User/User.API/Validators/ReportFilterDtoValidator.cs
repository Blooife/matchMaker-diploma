using FluentValidation;
using User.DataAccess.Dtos;

namespace User.API.Validators;

public class ReportFilterDtoValidator : AbstractValidator<ReportFilterDto>
{
    public ReportFilterDtoValidator()
    {
        RuleFor(x => x.CreatedTo)
            .GreaterThanOrEqualTo(x => x.CreatedFrom);
    }
}