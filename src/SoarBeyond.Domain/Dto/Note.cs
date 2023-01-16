using FluentValidation;
using SoarBeyond.Data;

namespace SoarBeyond.Domain.Dto;

public class Note
{
    public int Id { get; set; }
    public string Details { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public int MomentId { get; set; }

    public class Validator : AbstractValidator<Note>
    {
        public Validator()
        {
            RuleFor(n => n.Details)
                .NotEmpty()
                .WithMessage("Please add some details")
                .MaximumLength(NoteConstraints.DetailsLength)
                .WithMessage($"{{PropertyName}} can only be {NoteConstraints.DetailsLength} characters long");
        }
    }
}