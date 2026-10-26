namespace Prothia.API.Patients.Domain.Model.ValueObjects;

public sealed record PatientId
{
    private const int MaxLength = 50;
    
    public string Value { get; }
    
    public PatientId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PatientId cannot be null or empty.", nameof(value));
        
        if (value.Length > MaxLength)
            throw new ArgumentException($"PatientId cannot exceed {MaxLength} characters.", nameof(value));

        Value = value;
    }
}