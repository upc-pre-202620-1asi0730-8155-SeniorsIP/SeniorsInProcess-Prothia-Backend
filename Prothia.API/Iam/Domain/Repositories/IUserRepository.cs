using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Iam.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
}