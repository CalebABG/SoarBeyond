using System.Text.RegularExpressions;
using FluentValidation;

namespace SoarBeyond.Shared.Dto.Validators;

public class ThoughtValidator : AbstractValidator<Thought>
{
    private const int DetailsLength = 450;
    private const string HexColorRegexPattern = @"^#([0-9a-fA-F]{8}|[0-9a-fA-F]{6}|[0-9a-fA-F]{4}|[0-9a-fA-F]{3})$";

    private static readonly Regex HexColorRegex = new(HexColorRegexPattern,
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(250));

    public ThoughtValidator()
    {
        RuleFor(dto => dto.Details)
            .NotEmpty()
            .WithMessage("Please add some details")
            .MaximumLength(DetailsLength)
            .WithMessage($"{{PropertyName}} can only be {DetailsLength} characters long");

        RuleFor(dto => dto.Color)
            .Must(BeAValidHexColor)
            .WithMessage($"{{PropertyName}} must be a valid hex color");
    }

    private static bool BeAValidHexColor(string s) => HexColorRegex.IsMatch(s);
}