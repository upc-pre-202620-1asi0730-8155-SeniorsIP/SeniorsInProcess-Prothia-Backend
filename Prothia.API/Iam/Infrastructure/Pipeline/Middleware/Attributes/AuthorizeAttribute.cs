namespace Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
}