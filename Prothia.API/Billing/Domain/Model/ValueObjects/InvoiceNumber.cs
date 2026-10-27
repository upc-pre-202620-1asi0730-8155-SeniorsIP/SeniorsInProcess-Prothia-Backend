using System.Text.RegularExpressions;

namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public sealed record InvoiceNumber
{
    private static readonly Regex InvoiceRegex = new(@"^[A-Z0-9]{4}-\d{6,8}$", RegexOptions.Compiled);
    
    public string Value { get; }

    public InvoiceNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice number cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();

        if (!InvoiceRegex.IsMatch(trimmed))
            throw new ArgumentException($"Invalid invoice number format '{value}'. Expected format like 'F001-000842'.", nameof(value));
        Value = trimmed;
    }

    public string Series => Value.Split('-')[0];
    public string Correlative => Value.Split('-')[1];

    public override string ToString() => Value;
    
    public static implicit operator string (InvoiceNumber invoiceNumber) => invoiceNumber.Value;    
}
    
