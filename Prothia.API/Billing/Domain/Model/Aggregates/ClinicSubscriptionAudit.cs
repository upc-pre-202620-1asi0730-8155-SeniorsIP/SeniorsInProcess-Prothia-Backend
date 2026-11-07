using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Billing.Domain.Model.Aggregates;

public partial class ClinicSubscription : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}