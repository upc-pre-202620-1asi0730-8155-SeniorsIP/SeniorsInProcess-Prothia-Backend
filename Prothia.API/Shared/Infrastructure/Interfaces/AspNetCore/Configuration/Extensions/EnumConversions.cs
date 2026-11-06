namespace Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

/// <summary>
///     Shared helper for converting between domain enumerations and API string representations.
/// </summary>
public static class EnumConversions
{
    /// <summary>
    ///     Parses a string value into a domain enum value (case-insensitive, ignoring hyphens and underscores).
    /// </summary>
    public static TEnum Parse<TEnum>(string? value, string paramName) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

        var normalized = value.Trim().Replace("-", string.Empty).Replace("_", string.Empty);

        if (Enum.TryParse<TEnum>(normalized, ignoreCase: true, out var parsed) && Enum.IsDefined(parsed))
            return parsed;

        throw new ArgumentException($"Invalid {paramName} value '{value}'.", paramName);
    }

    /// <summary>
    ///     Formats a domain enum value as a camelCase string (e.g. <c>InProgress</c> -> <c>inProgress</c>).
    /// </summary>
    public static string ToCamelCaseString<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var name = value.ToString();
        return string.IsNullOrEmpty(name) ? name : char.ToLowerInvariant(name[0]) + name[1..];
    }
}