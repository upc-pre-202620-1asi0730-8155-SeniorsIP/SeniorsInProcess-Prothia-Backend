using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public sealed class SignUpResource
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9._-]{3,50}$")]
    public string Username { get; init; } = string.Empty;

    [Required]
    [RegularExpression("(?i)^(Physiotherapist|Technician)$")]
    public string Role { get; init; } = string.Empty;

    [Required]
    [MinLength(12)]
    [MaxLength(72)]
    public string Password { get; init; } = string.Empty;
}
