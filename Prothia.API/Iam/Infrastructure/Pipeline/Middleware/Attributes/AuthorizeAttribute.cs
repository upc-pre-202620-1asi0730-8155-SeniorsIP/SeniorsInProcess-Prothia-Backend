using Prothia.API.Iam.Domain.Model;

namespace Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeAttribute(params ERole[] allowedRoles)
    {
        if (allowedRoles.Length > 0)
            Roles = string.Join(",", allowedRoles.Select(role => role.ToString()));
    }
}