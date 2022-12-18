using System.Text.RegularExpressions;
using FluentValidation;
using SoarBeyond.Shared;

namespace SoarBeyond.Domain.Dto.Validators;

public class MomentValidator : AbstractValidator<Moment>
{
    private const string HexColorRegexPattern = @"^#([0-9a-fA-F]{8}|[0-9a-fA-F]{6}|[0-9a-fA-F]{4}|[0-9a-fA-F]{3})$";

    private static readonly Regex HexColorRegex = new(HexColorRegexPattern,
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(250));

    public MomentValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .WithMessage("Please add a title")
            .MaximumLength(MomentConstraints.TitleLength)
            .WithMessage($"{{PropertyName}} can only be {MomentConstraints.TitleLength} characters long");

        RuleFor(dto => dto.Color)
            .Must(BeAValidHexColor)
            .WithMessage($"{{PropertyName}} must be a valid hex color");

        RuleFor(dto => dto.Content)
            .NotEmpty()
            .WithMessage("Please add some content")
            .MaximumLength(MomentConstraints.DescriptionLength)
            .WithMessage($"{{PropertyName}} can only be {MomentConstraints.DescriptionLength} characters long");
    }

    private static bool BeAValidHexColor(string s) => HexColorRegex.IsMatch(s);
}