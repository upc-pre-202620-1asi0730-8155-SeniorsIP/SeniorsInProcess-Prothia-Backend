using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Workshop.Domain.Model.Aggregates;

public partial class Prosthesis : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}