using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Analytics.Domain.Model.Entities;

public partial class ReportIndicator : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}