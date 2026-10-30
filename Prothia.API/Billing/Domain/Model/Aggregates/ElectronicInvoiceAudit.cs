using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Billing.Domain.Model.Aggregates;

public partial class ElectronicInvoice : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}