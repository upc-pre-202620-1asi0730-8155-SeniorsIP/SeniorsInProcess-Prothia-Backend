namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
/// Represents the professional licensing registration code of a certifying physiotherapist (e.g. 'CTMP 14820').
/// </summary>
public sealed record TherapistRegistry
{
    private const int MaxLength = 50;

    public string Value { get; }

    public TherapistRegistry(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Therapist registry code cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Therapist registry code cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(TherapistRegistry registry) => registry.Value;
}