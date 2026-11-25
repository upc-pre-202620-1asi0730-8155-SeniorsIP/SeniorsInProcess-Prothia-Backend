namespace Prothia.API.Iam.Application.Model;

public sealed record AuthenticatedUser(
    int UserId,
    string Username,
    string Role,
    string Token,
    DateTimeOffset ExpiresAt);

public sealed record IssuedToken(string Value, DateTimeOffset ExpiresAt);

public sealed record IamUserIdentity(int UserId, string Username, string Role);
