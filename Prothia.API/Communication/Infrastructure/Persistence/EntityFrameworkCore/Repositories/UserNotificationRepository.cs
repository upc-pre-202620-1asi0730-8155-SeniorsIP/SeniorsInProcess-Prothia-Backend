using Microsoft.EntityFrameworkCore;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserNotificationRepository(AppDbContext context) : BaseRepository<UserNotification>(context), IUserNotificationRepository
{
    public async Task<IEnumerable<UserNotification>> FindByUserIdAsync(Guid userId)
    {
        // Trae las notificaciones de un usuario, ordenadas por las más recientes
        return await Context.Set<UserNotification>()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Id) 
            .ToListAsync();
    }

    public async Task<UserNotification?> FindByIdAsync(Guid notificationId)
    {
        return await Context.Set<UserNotification>()
            .FirstOrDefaultAsync(n => n.Id == notificationId);
    }
}