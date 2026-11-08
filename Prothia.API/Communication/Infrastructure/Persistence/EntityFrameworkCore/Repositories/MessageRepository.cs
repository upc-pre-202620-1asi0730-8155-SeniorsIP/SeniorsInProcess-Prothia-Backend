using Microsoft.EntityFrameworkCore;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MessageRepository(AppDbContext context) : BaseRepository<Message>(context), IMessageRepository
{
    public async Task<IEnumerable<Message>> GetConversationAsync(Guid userId, Guid otherUserId)
    {
        // Trae todos los mensajes entre dos usuarios y los ordena cronológicamente
        return await Context.Set<Message>()
            .Where(m => (m.SenderUserId == userId && m.RecipientUserId == otherUserId) ||
                        (m.SenderUserId == otherUserId && m.RecipientUserId == userId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }
}