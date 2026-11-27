using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Iam.Interfaces.Rest.Resources;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class CreateManagedUserCommandFromResourceAssembler
{
    public static CreateManagedUserCommand ToCommandFromResource(CreateManagedUserResource resource) =>
        new(resource.Username, resource.Password, resource.FullName, resource.Email, resource.Role, resource.PatientId);
}
