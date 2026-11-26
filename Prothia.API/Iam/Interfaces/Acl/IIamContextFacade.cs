using Prothia.API.Iam.Application.Model;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<IamUserIdentity?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IamUserIdentity?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Result<IamUserIdentity>> CreatePatientAccountAsync(
        int patientId,
        string username,
        string password,
        CancellationToken cancellationToken = default);
}
