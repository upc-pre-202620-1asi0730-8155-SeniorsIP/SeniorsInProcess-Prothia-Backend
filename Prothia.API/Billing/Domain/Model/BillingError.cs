namespace Prothia.API.Billing.Domain.Model;

public enum BillingError
{
    None,
    ClinicSubscriptionNotFound,
    ElectronicInvoiceNotFound,
    DuplicateInvoiceNumber,
    InvalidBillingData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}