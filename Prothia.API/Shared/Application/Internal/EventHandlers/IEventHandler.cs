using Cortex.Mediator.Notifications;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}
