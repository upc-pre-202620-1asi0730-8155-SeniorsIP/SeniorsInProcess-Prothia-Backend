using Prothia.API.Iam.Domain.Model.ValueObjects;

namespace Prothia.API.Iam.Domain.Model.Aggregates;

public sealed record UserAudit(UserId UserId, string Action, DateTimeOffset OccurredAt);