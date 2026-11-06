using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Billing.Domain.Model.Errors;

public static class BillingErrors
{
    public static readonly Error ClinicSubscriptionNotFound = Error.NotFound(
        "Billing.ClinicSubscriptionNotFound",
        "The specified clinic subscription was not found.");

    public static readonly Error ElectronicInvoiceNotFound = Error.NotFound(
        "Billing.ElectronicInvoiceNotFound",
        "The specified electronic invoice was not found.");

    public static readonly Error DuplicateInvoiceNumber = Error.Conflict(
        "Billing.DuplicateInvoiceNumber",
        "An electronic invoice with the specified invoice number already exists.");

    public static readonly Error InvalidBillingData = Error.Validation(
        "Billing.InvalidBillingData",
        "The provided billing data is invalid.");
}