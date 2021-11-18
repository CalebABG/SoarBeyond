using System;
using System.Linq.Expressions;
using Humanizer;

namespace SoarBeyond.Shared.Extensions;

public static class CommonExtensions
{
    private const LetterCasing DefaultLetterCasing = LetterCasing.Title;

    public static string LabelFor(string @string, LetterCasing letterCasing = DefaultLetterCasing)
        => @string.Humanize(letterCasing);

    public static string LabelFor<T>(Expression<Func<T>> expression, LetterCasing letterCasing = DefaultLetterCasing)
    {
        if (expression.Body is not MemberExpression me)
        {
            throw new ArgumentException("You must pass a lambda of the form: " +
                                        "'() => Class.Property' or '() => object.Property'");
        }

        return LabelFor(me.Member.Name, letterCasing);
    }
}