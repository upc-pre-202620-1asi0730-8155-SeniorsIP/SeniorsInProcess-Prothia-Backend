using Prothia.API.Iam.Application.QueryServices;
using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Domain.Model.Queries;
using Prothia.API.Iam.Interfaces.Acl;

namespace Prothia.API.Iam.Application.Acl;

public sealed class IamContextFacade(IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<IamUserIdentity?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);
        return user is null ? null : new IamUserIdentity(user.Id.Value, user.Username, user.Role);
    }

    public async Task<IamUserIdentity?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var user = await userQueryService.Handle(new GetUserByUsernameQuery(username), cancellationToken);
        return user is null ? null : new IamUserIdentity(user.Id.Value, user.Username, user.Role);
    }
}
