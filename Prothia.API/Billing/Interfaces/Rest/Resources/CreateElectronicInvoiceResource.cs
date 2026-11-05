using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Billing.Interfaces.Rest.Resources;

public record CreateElectronicInvoiceResource(
    [Required(ErrorMessage = "RequiredInvoiceNumber")]
    string InvoiceNumber,
    [Required(ErrorMessage = "RequiredInvoiceIssueDate")]
    string IssueDate,
    [Required(ErrorMessage = "RequiredInvoiceConcept")]
    string Concept,
    [Range(0, double.MaxValue, ErrorMessage = "InvalidInvoiceAmount")]
    decimal Amount,
    [Required(ErrorMessage = "RequiredInvoiceCurrency")]
    string Currency,
    [Required(ErrorMessage = "RequiredInvoiceStatus")]
    string Status,
    [Required(ErrorMessage = "RequiredInvoiceStatusLabel")]
    string StatusLabel,
    [Required(ErrorMessage = "RequiredInvoiceDownloadUrl")]
    string DownloadUrl);