using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Billing.Interfaces.Rest.Resources;

public record PaymentMethodResource(
    [Required(ErrorMessage = "RequiredPaymentMethodBrand")]
    string Brand,
    [Required(ErrorMessage = "RequiredPaymentMethodCardTitle")]
    string CardTitle,
    [Required(ErrorMessage = "RequiredPaymentMethodExpiry")]
    string Expiry,
    bool AutoRenew = true);