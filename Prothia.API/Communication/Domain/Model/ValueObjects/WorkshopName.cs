namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
///     Name of the orthoprosthetic workshop receiving a clinical data transmission.
/// </summary>
public sealed record WorkshopName
{
    public const int MaxLength = 150;

    public string Value { get; }

    public WorkshopName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("WorkshopName cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"WorkshopName cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}