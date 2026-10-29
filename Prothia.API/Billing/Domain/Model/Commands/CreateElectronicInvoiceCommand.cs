using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Commands;

public record CreateElectronicInvoiceCommand(
    InvoiceNumber InvoiceNumber,
    string IssuaDate,
    string Concept,
    decimal Amount,
    string currency,
    InvoiceStatus Status,
    string StatusLabel,
    string DownloadUrl);