using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto.Validators;

public class ReflectionValidator : AbstractValidator<Reflection>
{
    public ReflectionValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .WithMessage("Please add a title")
            .MaximumLength(ReflectionConstraints.TitleLength)
            .WithMessage($"{{PropertyName}} can only be {ReflectionConstraints.TitleLength} characters long");

        RuleFor(dto => dto.Details)
            .NotEmpty()
            .WithMessage("Please add some details")
            .MaximumLength(ReflectionConstraints.DetailsLength)
            .WithMessage($"{{PropertyName}} can only be {ReflectionConstraints.DetailsLength} characters long");
    }
}