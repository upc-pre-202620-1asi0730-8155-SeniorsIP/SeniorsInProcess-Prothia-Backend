namespace Prothia.API.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a clinical message.
/// </summary>
public record ClinicalMessageResource(
    int Id,
    string Sender,
    string SenderName,
    string SenderRole,
    string Text,
    string Timestamp,
    bool IsSystem,
    string Status);