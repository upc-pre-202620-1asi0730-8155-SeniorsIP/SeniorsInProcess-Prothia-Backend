using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Interfaces.Rest.Resources;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User entity, string token) =>
        new(
            entity.Id.Value,
            entity.Username,
            entity.FullName,
            entity.Email,
            entity.Role.ToString(),
            entity.PatientId,
            token);
}
