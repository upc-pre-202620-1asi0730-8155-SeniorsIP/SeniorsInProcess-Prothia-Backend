namespace Prothia.API.Billing.Domain.Model.ValueObjects;

/// <summary>
///     Encapsulates billing payment method details (card brand, masked descriptor, expiry, and auto-renew flag).
/// </summary>
public sealed record PaymentMethod
{
    public const int BrandMaxLength = 50;
    public const int CardTitleMaxLength = 100;
    public const int ExpiryMaxLength = 20;

    public string Brand { get; }
    public string CardTitle { get; }
    public string Expiry { get; }
    public bool AutoRenew { get; }

    private PaymentMethod()
    {
        Brand = string.Empty;
        CardTitle = string.Empty;
        Expiry = string.Empty;
    }

    public PaymentMethod(string brand, string cardTitle, string expiry, bool autoRenew = true)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Card brand cannot be null or whitespace.", nameof(brand));
        if (string.IsNullOrWhiteSpace(cardTitle))
            throw new ArgumentException("Card title cannot be null or whitespace.", nameof(cardTitle));
        if (string.IsNullOrWhiteSpace(expiry))
            throw new ArgumentException("Card expiry cannot be null or whitespace.", nameof(expiry));

        var trimmedBrand = brand.Trim();
        var trimmedTitle = cardTitle.Trim();
        var trimmedExpiry = expiry.Trim();

        if (trimmedBrand.Length > BrandMaxLength)
            throw new ArgumentException($"Brand cannot exceed {BrandMaxLength} characters.", nameof(brand));
        if (trimmedTitle.Length > CardTitleMaxLength)
            throw new ArgumentException($"Card title cannot exceed {CardTitleMaxLength} characters.",
                nameof(cardTitle));
        if (trimmedExpiry.Length > ExpiryMaxLength)
            throw new ArgumentException($"Expiry cannot exceed {ExpiryMaxLength} characters.", nameof(expiry));

        Brand = trimmedBrand;
        CardTitle = trimmedTitle;
        Expiry = trimmedExpiry;
        AutoRenew = autoRenew;
    }

    public override string ToString() => $"{Brand} - {CardTitle} (Exp: {Expiry})";
}
