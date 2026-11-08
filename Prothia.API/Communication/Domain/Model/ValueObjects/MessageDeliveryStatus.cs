namespace Prothia.API.Communication.Domain.Model.ValueObjects;

/// <summary>
///     Delivery status of a clinical message.
/// </summary>
public enum MessageDeliveryStatus
{
    Sent,
    Delivered,
    Read
}