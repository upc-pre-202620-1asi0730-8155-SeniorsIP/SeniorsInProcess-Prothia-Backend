namespace Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public sealed class AllowAnonymousAttribute : Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute
{
}