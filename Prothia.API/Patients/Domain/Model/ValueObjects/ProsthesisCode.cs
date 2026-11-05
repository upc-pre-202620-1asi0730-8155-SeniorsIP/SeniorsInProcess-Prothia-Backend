namespace Prothia.API.Patients.Domain.Model.ValueObjects;

public sealed record ProsthesisCode
{
    public const int MaxLength = 50;

    public string Value { get; }

    public ProsthesisCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ProsthesisCode cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"ProsthesisCode cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;
}
