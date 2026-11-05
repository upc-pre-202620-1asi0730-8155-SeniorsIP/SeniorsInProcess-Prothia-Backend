namespace Prothia.API.Billing.Interfaces.Rest.Resources;

public record ElectronicInvoiceResource(
    int Id,
    string InvoiceNumber,
    string IssueDate,
    string Concept,
    decimal Amount,
    string Currency,
    string Status,
    string StatusLabel,
    string DownloadUrl);