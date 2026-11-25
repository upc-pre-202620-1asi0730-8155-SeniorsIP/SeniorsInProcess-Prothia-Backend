using Prothia.API.Iam.Application.Model;

namespace Prothia.API.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<IamUserIdentity?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IamUserIdentity?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
