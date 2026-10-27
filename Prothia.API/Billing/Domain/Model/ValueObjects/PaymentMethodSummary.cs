using System.Text.RegularExpressions;

namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public sealed record PaymentMethodSummary
{
    private const int MaxBrandLength = 50;
    private const int MaxTitleLength = 100;
    private static readonly Regex ExpiryRegex = new(@"^(0[1-9]|1[0-2])\/\d{2}$", RegexOptions.Compiled);

    public string Brand { get; }
    public string CardTitle { get; }
    public string Expiry { get; }
    public bool AutoRenew { get; }

    public PaymentMethodSummary(string brand, string cardTitle, string expiry, bool autoRenew = true)
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

        if (trimmedBrand.Length > MaxBrandLength)
            throw new ArgumentException($"Brand cannot exceed {MaxBrandLength} characters.", nameof(brand));

        if (trimmedTitle.Length > MaxTitleLength)
            throw new ArgumentException($"Card title cannot exceed {MaxTitleLength} characters.", nameof(cardTitle));

        if (!ExpiryRegex.IsMatch(trimmedExpiry))
            throw new ArgumentException($"Invalid expiry format '{expiry}'. Expected format MM/YY (e.g. '08/29').", nameof(expiry));

        Brand = trimmedBrand;
        CardTitle = trimmedTitle;
        Expiry = trimmedExpiry;
        AutoRenew = autoRenew;
    }

    public override string ToString() => $"{Brand} - {CardTitle} (Exp: {Expiry})";
}
