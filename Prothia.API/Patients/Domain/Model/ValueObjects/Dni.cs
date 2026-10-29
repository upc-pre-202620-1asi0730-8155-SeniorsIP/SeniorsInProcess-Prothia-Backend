namespace Prothia.API.Patients.Domain.Model.ValueObjects;

public sealed record Dni
{
    public const int Length = 8;
    
    public string Value { get; }

    public Dni(string value) 
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Dni cannot be null or whitespace.", nameof(value));
        
        var trimmed = value.Trim();
        if (trimmed.Length != Length || !trimmed.All(char.IsAsciiDigit))
            throw new ArgumentException($"Dni must contain exactly {Length} digits.", nameof(value));
        
        Value = trimmed;
    }

    public override string ToString() => Value;

    public static string MaskedForLogging(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        
        var visibleChars = Math.Min(3, value.Length);
        var maskedPart = new string('*', value.Length - visibleChars);
        return maskedPart + value[^visibleChars..];
    }
    
    public string MaskedForLogging() => MaskedForLogging(Value);
}