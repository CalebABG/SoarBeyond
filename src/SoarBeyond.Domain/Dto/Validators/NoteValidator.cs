using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto.Validators;

public class NoteValidator : AbstractValidator<Note>
{
    public NoteValidator()
    {
        RuleFor(dto => dto.Details)
            .NotEmpty()
            .WithMessage("Please add some details")
            .MaximumLength(NoteConstraints.DetailsLength)
            .WithMessage($"{{PropertyName}} can only be {NoteConstraints.DetailsLength} characters long");
    }
}