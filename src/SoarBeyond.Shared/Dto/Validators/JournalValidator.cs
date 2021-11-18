using FluentValidation;

namespace SoarBeyond.Shared.Dto.Validators;

public class JournalValidator : AbstractValidator<Journal>
{
    private const int NameLength = 120;
    private const int DescriptionLength = 450;

    public JournalValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("Please add a name")
            .MaximumLength(NameLength)
            .WithMessage($"{{PropertyName}} can only be {NameLength} characters long");

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Please add a description")
            .MaximumLength(DescriptionLength)
            .WithMessage($"{{PropertyName}} can only be {DescriptionLength} characters long");
    }
}