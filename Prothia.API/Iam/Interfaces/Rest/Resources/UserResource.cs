namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public record UserResource(
    int Id,
    string Username,
    string FullName,
    string Email,
    string Role,
    int? PatientId,
    bool IsActive,
    DateTimeOffset? CreatedAt,
    int? CreatedByUserId = null);
