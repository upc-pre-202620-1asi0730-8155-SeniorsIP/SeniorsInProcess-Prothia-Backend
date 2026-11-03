using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;

namespace Prothia.API.Communication.Application.Services;

public interface ICommunicationNotificationQueryService
{
    Task<IEnumerable<Message>> Handle(GetConversationQuery query);
    Task<IEnumerable<UserNotification>> Handle(GetNotificationsByUserIdQuery query);
}