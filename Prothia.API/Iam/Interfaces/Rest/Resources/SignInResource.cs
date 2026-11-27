using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public record SignInResource(
    [Required(ErrorMessage = "Username is required.")]
    string Username,
    [Required(ErrorMessage = "Password is required.")]
    string Password);
