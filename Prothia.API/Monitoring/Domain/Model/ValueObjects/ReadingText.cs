namespace Prothia.API.Monitoring.Domain.Model.ValueObjects;

public sealed record ReadingText
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    public ReadingText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ReadingText cannot be null or whitespace.", nameof(value));
        
        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"ReadingText cannot exceed {MaxLength} characters.", nameof(value));
        
        Value = trimmed;
    }
    
    public override string ToString() => Value;
}