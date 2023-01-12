﻿using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto;

public class Journal
{
    public int Id { get; set; }
    public bool Favored { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public int UserId { get; set; }

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
                .NotEmpty()
                .WithMessage("Please add a description")
                .MaximumLength(JournalConstraints.DescriptionLength)
                .WithMessage($"{{PropertyName}} can only be {JournalConstraints.DescriptionLength} characters long");
        }
    }
}