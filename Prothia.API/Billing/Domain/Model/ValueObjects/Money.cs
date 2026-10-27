namespace Prothia.API.Billing.Domain.Model.ValueObjects;


public sealed record Money
{
    public const string DefaultCurrency = "USD";
    
    public decimal Amount { get; }
    
    public string Currency { get; }

    public Money(decimal amount, string currency = DefaultCurrency)
    {
        if(amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Monetary amount cannot be negative");
        
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or whitespace.", nameof(currency));
        
        var trimmedCurrency = currency.Trim().ToUpperInvariant();
        if(trimmedCurrency.Length is < 2 or > 4)
            throw new ArgumentException($"Currency code '{currency}' must be 2 to 4 characters long (e.g. USD, PEN).", nameof(currency));
        Amount = Math.Round(amount, 2);
        Currency = trimmedCurrency;
    }

}
