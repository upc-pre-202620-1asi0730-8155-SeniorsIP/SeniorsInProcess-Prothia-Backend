using System.Text.RegularExpressions;

namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
/// Represents a Peruvian Tax Identification Number (Registro Único de Contribuyentes - RUC).
/// Consists of exactly 11 numeric digits, typically starting with '10' or '20'.
/// </summary>
public sealed record TaxId
{
    private const int RequiredLength = 11;
    private static readonly Regex RucRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public string Value { get; }

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tax ID cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();

        if (trimmed.Length != RequiredLength || !RucRegex.IsMatch(trimmed))
            throw new ArgumentException($"Tax ID (RUC) must consist of exactly {RequiredLength} numeric digits.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(TaxId taxId) => taxId.Value;
}