using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Queries;

public record GetAllAlertsQuery(PatientId? PatientId = null, EAlertStatus? Status = null);