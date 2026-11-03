using System.ComponentModel.DataAnnotations.Schema;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

public partial class UserNotification
{
    [Column("created_at")] public DateTimeOffset? CreatedAt { get; set; }
    [Column("updated_at")] public DateTimeOffset? UpdatedAt { get; set; }
}