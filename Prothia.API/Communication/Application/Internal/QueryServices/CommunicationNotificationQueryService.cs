using Prothia.API.Communication.Application.Services;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.Queries;
using Prothia.API.Communication.Domain.Repositories;

namespace Prothia.API.Communication.Application.Internal.QueryServices;

public class CommunicationNotificationQueryService(
    IMessageRepository messageRepository,
    IUserNotificationRepository userNotificationRepository) : ICommunicationNotificationQueryService
{
    public async Task<IEnumerable<Message>> Handle(GetConversationQuery query)
    {
        return await messageRepository.GetConversationAsync(query.UserId, query.OtherUserId);
    }

    public async Task<IEnumerable<UserNotification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await userNotificationRepository.FindByUserIdAsync(query.UserId);
    }
}