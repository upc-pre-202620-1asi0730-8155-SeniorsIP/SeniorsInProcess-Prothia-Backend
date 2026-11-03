using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

public partial class UserNotification : IAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public Guid? ReferenceId { get; private set; }
    public DateTime? ReadAt { get; private set; }

    protected UserNotification() {}

    public UserNotification(Guid userId, NotificationType type, Guid? referenceId = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Type = type;
        ReferenceId = referenceId;
        ReadAt = null;
    }

    public void MarkRead()
    {
        if (ReadAt == null) ReadAt = DateTime.UtcNow;
    }
}