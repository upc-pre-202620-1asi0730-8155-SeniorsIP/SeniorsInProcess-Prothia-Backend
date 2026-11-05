using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Analytics.Domain.Model.Aggregates;

public partial class ClinicalReport : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}