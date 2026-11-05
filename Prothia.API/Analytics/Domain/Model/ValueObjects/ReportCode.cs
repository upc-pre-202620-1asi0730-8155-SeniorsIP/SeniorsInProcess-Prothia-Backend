namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
///     Represents the unique official code of a clinical report (e.g. 'EXP-CLIN-2026-084').
/// </summary>
public sealed record ReportCode
{
    public const int MaxLength = 50;

    public string Value { get; }

    public ReportCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Report code cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Report code cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(ReportCode code) => code.Value;
}