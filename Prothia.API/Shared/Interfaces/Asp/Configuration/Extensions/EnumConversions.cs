namespace Prothia.API.Shared.Interfaces.Asp.Configuration.Extensions;

/// <summary>
///     Conversions between enumerations and the strings exposed by the REST resources.
/// </summary>
public static class EnumConversions
{
    /// <summary>
    ///     Parses an enumeration name case-insensitively. Numeric values are rejected so clients
    ///     cannot depend on the underlying integers.
    /// </summary>
    /// <exception cref="ArgumentException">When the value is not a defined member of the enumeration.</exception>
    public static TEnum Parse<TEnum>(string? value, string parameterName) where TEnum : struct, Enum
    {
        var trimmed = value?.Trim();
        if (string.IsNullOrEmpty(trimmed) || char.IsDigit(trimmed[0]) ||
            !Enum.TryParse<TEnum>(trimmed, true, out var result) || !Enum.IsDefined(result))
            throw new ArgumentException($"'{value}' is not a valid {typeof(TEnum).Name}.", parameterName);
        return result;
    }

    /// <summary>Formats an enumeration member in camelCase (e.g. <c>TranstibialRight</c> becomes <c>transtibialRight</c>).</summary>
    public static string ToCamelCaseString<TEnum>(this TEnum value) where TEnum : struct, Enum
    {
        var name = value.ToString();
        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}