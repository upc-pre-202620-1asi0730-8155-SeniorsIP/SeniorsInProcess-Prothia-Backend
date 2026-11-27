namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public record AuthenticatedUserResource(
    int Id,
    string Username,
    string FullName,
    string Email,
    string Role,
    int? PatientId,
    string Token);
