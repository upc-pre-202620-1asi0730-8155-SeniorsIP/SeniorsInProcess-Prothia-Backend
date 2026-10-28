namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;
/*
 * Represents a therapeutic focus area or rehabilitation objective
 */

public sealed record TherapeuticFocus
{
    private const int MaxLength = 100;

    public string Value { get; }

    public TherapeuticFocus(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Therapeutic focus cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Therapeutic focus cannot exceed {MaxLength} characters.", nameof(value));
        Value = trimmed;
    }
    public override string ToString() => Value;
    public static implicit operator string (TherapeuticFocus focus)=> focus.Value;
}