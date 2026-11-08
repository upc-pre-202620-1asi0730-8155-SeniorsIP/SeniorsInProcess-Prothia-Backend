using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.Commands;

/// <summary>
///     Command to record a clinical message in the communication thread.
/// </summary>
public record CreateClinicalMessageCommand(
    MessageSender Sender,
    string SenderName,
    string SenderRole,
    MessageText Text,
    string Timestamp,
    bool IsSystem,
    MessageDeliveryStatus Status);