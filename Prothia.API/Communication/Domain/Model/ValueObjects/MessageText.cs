namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
///     Encapsulates the textual content of a clinical message between patients and specialists.
/// </summary>
public sealed record MessageText
{
    public const int MaxLength = 2000;

    public string Value { get; }

    public MessageText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("MessageText cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"MessageText cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}