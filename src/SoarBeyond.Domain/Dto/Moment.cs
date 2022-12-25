﻿using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto;

public class Moment
{
    public int Id { get; set; }
    public int JournalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Color { get; set; } = "#428bff";

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

    public virtual List<Note> Notes { get; set; } = new();
    public virtual List<Reflection> Reflections { get; set; } = new();

    public class Validator : AbstractValidator<Moment>
    {
        public Validator()
        {
            RuleFor(m => m.Title)
                .NotEmpty()
                .WithMessage("Please add a title")
                .MaximumLength(MomentConstraints.TitleLength)
                .WithMessage($"{{PropertyName}} can only be {MomentConstraints.TitleLength} characters long");

            RuleFor(m => m.Color)
                .Must(IsValidHexColor)
                .WithMessage($"{{PropertyName}} must be a valid hex color");

            RuleFor(m => m.Content)
                .NotEmpty()
                .WithMessage("Please add some content")
                .MaximumLength(MomentConstraints.DescriptionLength)
                .WithMessage($"{{PropertyName}} can only be {MomentConstraints.DescriptionLength} characters long");
        }

        private static bool IsValidHexColor(string s) => MomentConstraints.HexColorRegex.IsMatch(s);
    }
}