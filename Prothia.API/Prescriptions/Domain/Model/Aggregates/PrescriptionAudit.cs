using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

public partial class Prescription : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}