namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
/// Name of rehabilitation excercise in the clinical catalog
/// </summary>


public sealed record ExcerciseName
{
    public const int MaxLength = 150;
    public string Value { get; }

    public ExcerciseName(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException("Excersise Name cannot be null or whitespace", nameof(value));
        
        var trimmed=value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Excercise Name cannot exceeds {MaxLength} characters", nameof(value));
        Value = trimmed;
    }
    public override string ToString() => Value;
}