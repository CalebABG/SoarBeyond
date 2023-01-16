using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto;

public class Journal
{
    public int Id { get; set; }
    public bool Favorited { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public int UserId { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public List<Moment> Moments { get; set; } = new();

    public class Validator : AbstractValidator<Journal>
    {
        public Validator()
        {
            RuleFor(j => j.Name)
                .NotEmpty()
                .WithMessage("Please add a name")
                .MaximumLength(JournalConstraints.NameLength)
                .WithMessage($"{{PropertyName}} can only be {JournalConstraints.NameLength} characters long");

            RuleFor(j => j.Description)
                .MaximumLength(JournalConstraints.DescriptionLength)
                .WithMessage($"{{PropertyName}} can only be {JournalConstraints.DescriptionLength} characters long");
        }
    }
}