using Prothia.API.Iam.Domain.Model.Events;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;
using Prothia.API.Shared.Domain.Model.Entities;

namespace Prothia.API.Iam.Domain.Model.Aggregates;

public sealed class User : AggregateRoot, IAuditableEntity
{
    public const int UsernameMaxLength = 50;
    public const int PasswordHashMaxLength = 100;
    public const int RoleMaxLength = 30;
    public const string DefaultRole = "Fisioterapeuta";

    public UserId Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = DefaultRole;
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    private User()
    {
    }

    public User(string username, string passwordHash, string role = DefaultRole)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(role);

        Username = username.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        AddDomainEvent(new UserRegisteredEvent(Username));
    }
}
