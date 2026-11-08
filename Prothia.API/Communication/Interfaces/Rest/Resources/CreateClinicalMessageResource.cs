using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a clinical message.
/// </summary>
public record CreateClinicalMessageResource(
    [Required(ErrorMessage = "RequiredMessageSender")]
    string Sender,
    [Required(ErrorMessage = "RequiredMessageSenderName")]
    string SenderName,
    [Required(ErrorMessage = "RequiredMessageSenderRole")]
    string SenderRole,
    [Required(ErrorMessage = "RequiredMessageText")]
    string Text,
    string? Timestamp = null,
    bool IsSystem = false,
    string? Status = null);