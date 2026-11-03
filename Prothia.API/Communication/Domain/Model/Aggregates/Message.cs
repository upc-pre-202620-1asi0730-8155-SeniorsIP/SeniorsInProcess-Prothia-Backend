using Prothia.API.Shared.Domain.Model;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

public partial class Message : IAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid SenderUserId { get; private set; }
    public Guid RecipientUserId { get; private set; }
    public string Body { get; private set; }
    public DateTime SentAt { get; private set; }

    protected Message() { Body = string.Empty; }

    public Message(Guid senderUserId, Guid recipientUserId, string body)
    {
        Id = Guid.NewGuid();
        SenderUserId = senderUserId;
        RecipientUserId = recipientUserId;
        Body = body;
        SentAt = DateTime.UtcNow;
    }
}