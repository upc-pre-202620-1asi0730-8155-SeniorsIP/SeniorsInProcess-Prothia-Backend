using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Iam.Interfaces.Rest.Resources;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource) =>
        new(resource.Username, resource.Password);
}
