namespace Prothia.API.Monitoring.Domain.Model.ValueObjects;

public sealed record DeviationType
{
    public const int MaxLength = 150;

    public string Value { get; }

    public DeviationType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("DeviationType cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"DeviationType cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}