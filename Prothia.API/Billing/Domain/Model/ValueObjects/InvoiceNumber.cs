namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public sealed record InvoiceNumber
{
    public const int MaxLength = 30;

    public string Value { get; }

    public InvoiceNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice number cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"Invoice number cannot exceed {MaxLength} characters.", nameof(value));

        Value = trimmed;
    }

    public override string ToString() => Value;

    public static implicit operator string(InvoiceNumber invoiceNumber) => invoiceNumber.Value;
}