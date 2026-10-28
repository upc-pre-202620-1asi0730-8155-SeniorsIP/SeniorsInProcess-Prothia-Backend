namespace Prothia.API.Patients.Domain.Model.ValueObjects;

public sealed record FullName
{
    public const int MaxLength = 150;
    
    public string Value { get; }

    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FullName cannot be null or whitespace.", nameof(value));
        
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"FullName cannot be longer than {MaxLength} characters.", nameof(value));
        Value = trimmed;
    }
    
    public override string ToString() => Value;
}