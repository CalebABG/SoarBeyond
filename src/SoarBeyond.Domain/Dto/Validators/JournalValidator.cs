using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto.Validators;

public class JournalValidator : AbstractValidator<Journal>
{
    public JournalValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("Please add a name")
            .MaximumLength(JournalConstraints.NameLength)
            .WithMessage($"{{PropertyName}} can only be {JournalConstraints.NameLength} characters long");

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Please add a description")
            .MaximumLength(JournalConstraints.DescriptionLength)
            .WithMessage($"{{PropertyName}} can only be {JournalConstraints.DescriptionLength} characters long");
    }
}