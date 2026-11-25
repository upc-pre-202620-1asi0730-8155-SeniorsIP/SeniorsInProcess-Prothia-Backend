namespace Prothia.API.Iam.Interfaces.Rest.Resources;

public sealed record AuthenticatedUserResource(
    int Id,
    string Username,
    string Role,
    string AccessToken,
    string TokenType,
    DateTimeOffset ExpiresAt);