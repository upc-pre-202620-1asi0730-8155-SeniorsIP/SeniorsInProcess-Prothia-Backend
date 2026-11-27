using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Interfaces.Rest.Resources;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User entity) =>
        new(
            entity.Id.Value,
            entity.Username,
            entity.FullName,
            entity.Email,
            entity.Role.ToString(),
            entity.PatientId,
            entity.IsActive,
            entity.CreatedAt,
            entity.CreatedByUserId);
}
