namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
///     Name of a rehabilitation phase in a patient's prescription plan.
/// </summary>

public sealed record PhaseName
{
    public const int MaxLength = 50;
    public string Value { get; }

    public PhaseName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PhaseName cannot be null or whitespace.", nameof(value));
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"PhaseName cannot exceed {MaxLength} characters.", nameof(value));
        Value = trimmed;
    }
    public override string ToString() => Value;
}