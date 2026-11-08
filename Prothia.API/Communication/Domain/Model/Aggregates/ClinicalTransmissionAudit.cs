using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

public partial class ClinicalTransmission : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}