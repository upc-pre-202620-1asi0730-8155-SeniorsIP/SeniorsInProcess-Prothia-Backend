namespace Prothia.API.Workshop.Domain.Model.ValueObjects;

public sealed record SerialNumber
{
    public const int MaxLength = 50;

    public string Value { get; }

    public SerialNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SerialNumber cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"SerialNumber cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}