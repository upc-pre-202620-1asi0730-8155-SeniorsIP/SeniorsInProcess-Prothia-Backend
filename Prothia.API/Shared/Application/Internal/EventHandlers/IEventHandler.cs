using Cortex.Mediator.Notifications;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Shared.Application.Internal.EventHandlers;

/// <summary>
///     Defines a handler for a domain event.
/// </summary>
/// <typeparam name="TEvent">The type of domain event being handled.</typeparam>
public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}