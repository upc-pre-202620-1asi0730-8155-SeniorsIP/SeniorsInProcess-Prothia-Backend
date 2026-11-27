namespace Prothia.API.Iam.Interfaces.Acl;

public interface IIamContextFacade
{
    Task<int> FetchUserIdByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<string> FetchUsernameByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsUserByIdAsync(int userId, CancellationToken cancellationToken = default);
}
