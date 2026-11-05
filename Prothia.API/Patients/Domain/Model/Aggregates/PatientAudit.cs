using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Patients.Domain.Model.Aggregates;

public partial class Patient : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}