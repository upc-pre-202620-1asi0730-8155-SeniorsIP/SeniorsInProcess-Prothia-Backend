using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

public partial class Exercise : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}