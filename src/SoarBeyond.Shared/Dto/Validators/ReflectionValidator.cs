using FluentValidation;

namespace SoarBeyond.Shared.Dto.Validators;

public class ReflectionValidator : AbstractValidator<Reflection>
{
    private const int DetailsLength = 450;
    private const int TitleLength = 120;

    public ReflectionValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .WithMessage("Please add a title")
            .MaximumLength(TitleLength)
            .WithMessage($"{{PropertyName}} can only be {TitleLength} characters long");

        RuleFor(dto => dto.Details)
            .NotEmpty()
            .WithMessage("Please add some details")
            .MaximumLength(DetailsLength)
            .WithMessage($"{{PropertyName}} can only be {DetailsLength} characters long");
    }
}