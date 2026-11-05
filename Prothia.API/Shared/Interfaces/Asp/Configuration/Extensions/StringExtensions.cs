using System.Text.RegularExpressions;

namespace Prothia.API.Shared.Interfaces.Asp.Configuration.Extensions;

public static partial class StringExtensions
{
    [GeneratedRegex("(?<!^)([A-Z])", RegexOptions.Compiled)]
    private static partial Regex UpperCaseLetterRegex();

    /// <summary>
    ///     Converts PascalCase text (e.g. <c>GaitSessions</c>) to kebab-case (<c>gait-sessions</c>).
    /// </summary>
    public static string ToKebabCase(this string text)
    {
        return string.IsNullOrEmpty(text)
            ? text
            : UpperCaseLetterRegex().Replace(text, "-$1").Trim().ToLowerInvariant();
    }
}