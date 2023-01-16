using System.Text.RegularExpressions;

namespace SoarBeyond.Data;

// Length calculated: Average English word is ~5 chars, 1 bytes per char

public static class JournalConstraints
{
    public const int NameLength = 5 * 250;
    public const int DescriptionLength = 5 * 500;
}

public static class MomentConstraints
{
    public const string HexColorRegexPattern = @"^#([0-9a-fA-F]{8}|[0-9a-fA-F]{6}|[0-9a-fA-F]{4}|[0-9a-fA-F]{3})$";

    public static readonly Regex HexColorRegex = new(HexColorRegexPattern,
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(250));

    public const int ColorHexLength = 8;
    public const int TitleLength = 5 * 250;
    public const int DescriptionLength = 5 * 2500;
}

public static class NoteConstraints
{
    public const int DetailsLength = 5 * 500;
}

public static class ReflectionConstraints
{
    public const int TitleLength = 5 * 250;
    public const int DetailsLength = 5 * 2500;
}

public static class CategoryConstraints
{
    public const int NameLength = 5 * 30;
    public const int DescriptionLength = 5 * 150;
}