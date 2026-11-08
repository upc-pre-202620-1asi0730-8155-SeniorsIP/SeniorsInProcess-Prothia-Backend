using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

/// <summary>
///     Clinical message exchanged in the interdisciplinary communication thread between patients, therapists, and technicians.
/// </summary>
public partial class ClinicalMessage : AggregateRoot
{
    public const int SenderNameMaxLength = 150;
    public const int SenderRoleMaxLength = 100;
    public const int TimestampMaxLength = 50;

    public ClinicalMessageId Id { get; private set; }
    public MessageSender Sender { get; private set; }
    public string SenderName { get; private set; }
    public string SenderRole { get; private set; }
    public MessageText Text { get; private set; }
    public string Timestamp { get; private set; }
    public bool IsSystem { get; private set; }
    public MessageDeliveryStatus Status { get; private set; }

    protected ClinicalMessage()
    {
        SenderName = string.Empty;
        SenderRole = string.Empty;
        Text = null!;
        Timestamp = string.Empty;
    }

    public ClinicalMessage(CreateClinicalMessageCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (!Enum.IsDefined(command.Sender))
            throw new ArgumentOutOfRangeException(nameof(command), command.Sender, "Invalid message sender.");
        if (!Enum.IsDefined(command.Status))
            throw new ArgumentOutOfRangeException(nameof(command), command.Status, "Invalid message delivery status.");
        if (string.IsNullOrWhiteSpace(command.SenderName))
            throw new ArgumentException("SenderName cannot be null or whitespace.", nameof(command));
        if (command.SenderName.Trim().Length > SenderNameMaxLength)
            throw new ArgumentException($"SenderName cannot exceed {SenderNameMaxLength} characters.", nameof(command));
        if (string.IsNullOrWhiteSpace(command.SenderRole))
            throw new ArgumentException("SenderRole cannot be null or whitespace.", nameof(command));
        if (command.SenderRole.Trim().Length > SenderRoleMaxLength)
            throw new ArgumentException($"SenderRole cannot exceed {SenderRoleMaxLength} characters.", nameof(command));

        Sender = command.Sender;
        SenderName = command.SenderName.Trim();
        SenderRole = command.SenderRole.Trim();
        Text = command.Text;
        Timestamp = string.IsNullOrWhiteSpace(command.Timestamp)
            ? DateTimeOffset.UtcNow.ToString("HH:mm")
            : command.Timestamp.Trim();
        if (Timestamp.Length > TimestampMaxLength)
            throw new ArgumentException($"Timestamp cannot exceed {TimestampMaxLength} characters.", nameof(command));
        IsSystem = command.IsSystem;
        Status = command.Status;
    }
}
