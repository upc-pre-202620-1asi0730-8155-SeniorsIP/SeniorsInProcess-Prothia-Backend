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
    public UserId Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public int? PatientId { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    private User()
    {
    }

    public User(string username, string passwordHash, ERole role, int? patientId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        if (!Enum.IsDefined(role))
            throw new ArgumentOutOfRangeException(nameof(role));
        if ((role == ERole.Patient) != patientId.HasValue)
            throw new ArgumentException("Only a patient account must have a linked patient ID.", nameof(patientId));

        Username = username.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role.ToString();
        PatientId = patientId;
        AddDomainEvent(new UserRegisteredEvent(Username));
    }
}
