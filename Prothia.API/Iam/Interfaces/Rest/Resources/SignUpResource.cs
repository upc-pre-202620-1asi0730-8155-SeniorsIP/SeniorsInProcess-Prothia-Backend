using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public record SignUpResource(
    [Required(ErrorMessage = "Username is required.")]
    string Username,
    [Required(ErrorMessage = "Password is required.")]
    string Password,
    [Required(ErrorMessage = "Full name is required.")]
    string FullName,
    [Required(ErrorMessage = "Email is required.")]
    string Email,
    string? Role = null);
