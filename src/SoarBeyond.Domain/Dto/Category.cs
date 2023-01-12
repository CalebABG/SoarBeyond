using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public int UserId { get; set; }

    public class Validator : AbstractValidator<Category>
    {
        public Validator()
        {
            RuleFor(j => j.Name)
                .NotEmpty()
                .WithMessage("Please add a name")
                .MaximumLength(CategoryConstraints.NameLength)
                .WithMessage($"{{PropertyName}} can only be {CategoryConstraints.NameLength} characters long");
            
            RuleFor(j => j.Description)
                .MaximumLength(CategoryConstraints.DescriptionLength)
                .WithMessage($"{{PropertyName}} can only be {CategoryConstraints.DescriptionLength} characters long");
        }
    }
}