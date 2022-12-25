using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto;

public class Reflection
{
    public int Id { get; set; }
    public int MomentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public class Validator : AbstractValidator<Reflection>
    {
        public Validator()
        {
            RuleFor(r => r.Title)
                .NotEmpty()
                .WithMessage("Please add a title")
                .MaximumLength(ReflectionConstraints.TitleLength)
                .WithMessage($"{{PropertyName}} can only be {ReflectionConstraints.TitleLength} characters long");

            RuleFor(r => r.Details)
                .NotEmpty()
                .WithMessage("Please add some details")
                .MaximumLength(ReflectionConstraints.DetailsLength)
                .WithMessage($"{{PropertyName}} can only be {ReflectionConstraints.DetailsLength} characters long");
        }
    }
}