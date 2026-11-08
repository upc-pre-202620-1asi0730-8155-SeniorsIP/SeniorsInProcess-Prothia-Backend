using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Domain.Repositories;

public interface IUserNotificationRepository : IBaseRepository<UserNotification>
{
    Task<IEnumerable<UserNotification>> FindByUserIdAsync(Guid userId);
    Task<UserNotification?> FindByIdAsync(Guid notificationId);
}