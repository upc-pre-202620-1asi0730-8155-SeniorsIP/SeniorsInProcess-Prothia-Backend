using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Model.Queries;

/// <summary>
///     Query to retrieve mechanical alerts, optionally filtered by prosthesis or status.
/// </summary>
public record GetAllMechanicalAlertsQuery(int? ProsthesisId = null, MechanicalAlertStatus? Status = null);