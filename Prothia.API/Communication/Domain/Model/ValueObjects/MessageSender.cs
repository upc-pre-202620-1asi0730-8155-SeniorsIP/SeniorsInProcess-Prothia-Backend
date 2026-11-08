namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
///     Role category of the clinical message sender.
/// </summary>
public enum MessageSender
{
    User,
    Therapist,
    Technician,
    System
}