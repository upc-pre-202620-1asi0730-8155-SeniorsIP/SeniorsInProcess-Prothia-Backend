using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public sealed class SignInResource
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}
