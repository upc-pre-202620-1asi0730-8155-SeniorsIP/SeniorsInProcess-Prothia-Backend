using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Domain.Repositories;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<IEnumerable<Message>> GetConversationAsync(Guid userId, Guid otherUserId);
}