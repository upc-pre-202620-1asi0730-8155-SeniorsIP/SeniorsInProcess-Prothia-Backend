using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Commands;

public record CreateElectronicInvoiceCommand(
    InvoiceNumber InvoiceNumber,
    string IssueDate,
    string Concept,
    decimal Amount,
    string Currency,
    InvoiceStatus Status,
    string StatusLabel,
    string DownloadUrl);