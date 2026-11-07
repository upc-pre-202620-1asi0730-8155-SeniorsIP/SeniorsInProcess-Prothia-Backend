using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Monitoring.Domain.Model.Aggregates;

public partial class ThresholdProfile : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}