using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Prothia.API.Billing.Interfaces.Rest.Resources;
using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class CreateElectronicInvoiceCommandFromResourceAssembler
{
    public static CreateElectronicInvoiceCommand ToCommandFromResource(CreateElectronicInvoiceResource resource) =>
        new(
            new InvoiceNumber(resource.InvoiceNumber),
            resource.IssueDate,
            resource.Concept,
            resource.Amount,
            resource.Currency,
            EnumConversions.Parse<InvoiceStatus>(resource.Status, nameof(resource.Status)),
            resource.StatusLabel,
            resource.DownloadUrl);
}