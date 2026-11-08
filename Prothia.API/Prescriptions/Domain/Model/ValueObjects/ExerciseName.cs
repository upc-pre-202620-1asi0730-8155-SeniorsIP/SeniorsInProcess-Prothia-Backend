namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
///     Name of a rehabilitation exercise in the clinical catalog.
/// </summary>
public sealed record ExerciseName
{
    public const int MaxLength = 150;

    public string Value { get; }

    public ExerciseName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ExerciseName cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"ExerciseName cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}