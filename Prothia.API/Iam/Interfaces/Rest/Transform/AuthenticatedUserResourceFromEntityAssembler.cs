using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Interfaces.Rest.Resources;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(AuthenticatedUser authenticatedUser) =>
        new(
            authenticatedUser.UserId,
            authenticatedUser.Username,
            authenticatedUser.Role,
            authenticatedUser.Token,
            "Bearer",
            authenticatedUser.ExpiresAt);
}
