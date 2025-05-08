using FluentValidation;
using Profile.BusinessLogic.DTOs.Profile.Request;

namespace Profile.API.Validators;

public class CreateOrUpdateProfileDtoValidator : AbstractValidator<CreateOrUpdateProfileDto>
{
    public CreateOrUpdateProfileDtoValidator()
    {
        RuleFor(p => p.Name)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Имя должно быть от 2 до 50 символов");
        
        RuleFor(p => p.LastName)
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Фамилия должна быть от 2 до 50 символов");
        
        RuleFor(p => p.Bio)
            .MinimumLength(2)
            .MaximumLength(500)
            .WithMessage("О себе должно быть от 2 до 50 символов");
        
        RuleFor(p => p.Height)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(220)
            .WithMessage("Рост должен быть от 100 до 220 сантиметров");
        
        RuleFor(p => p.BirthDate)
            .LessThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Возраст должен быть больше 16 лет");
        
        RuleFor(p => p.MaxDistance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Максимальное расстояние должно быть больше 0");
        
        RuleFor(p => p.AgeFrom)
            .GreaterThanOrEqualTo(16)
            .WithMessage("Возраст партнёра от должен быть больше 16 лет");
        
        RuleFor(p => p.AgeTo)
            .GreaterThanOrEqualTo(16)
            .WithMessage("Возраст партнёра до должен быть больше 16 лет");
        
        RuleFor(p => p.AgeFrom)
            .LessThanOrEqualTo(p => p.AgeTo)
            .WithMessage("Возраст от должен быть меньше возраста до");
    }
}