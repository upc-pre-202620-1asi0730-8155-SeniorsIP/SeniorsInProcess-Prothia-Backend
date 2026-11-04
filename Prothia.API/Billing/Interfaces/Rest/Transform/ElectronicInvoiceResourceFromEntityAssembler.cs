using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Interfaces.Rest.Resources;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class ElectronicInvoiceResourceFromEntityAssembler
{
    public static ElectronicInvoiceResource ToResourceFromEntity(ElectronicInvoice entity)=>
        new(
            entity.Id,
            entity.InvoiceNumber.Value,
            entity.IssueDate,
            entity.Concept,
            entity.Amount,
            entity.Currency,
            entity.Status.ToCamelCaseString(),
            entity.StatusLabel,
            entity.DownloadUrl);
}