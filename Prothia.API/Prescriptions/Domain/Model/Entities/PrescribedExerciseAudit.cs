using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Prescriptions.Domain.Model.Entities;

public partial class PrescribedExercise : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}