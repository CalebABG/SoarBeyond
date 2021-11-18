using FluentValidation;

namespace SoarBeyond.Shared.Dto.Validators;

public class JournalEntryValidator : AbstractValidator<JournalEntry>
{
    private const int TitleLength = 120;
    private const int DescriptionLength = 450;

    public JournalEntryValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .WithMessage("Please add a title")
            .MaximumLength(TitleLength)
            .WithMessage($"{{PropertyName}} can only be {TitleLength} characters long");

        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Please add some content")
            .MaximumLength(DescriptionLength)
            .WithMessage($"{{PropertyName}} can only be {DescriptionLength} characters long");
    }
}