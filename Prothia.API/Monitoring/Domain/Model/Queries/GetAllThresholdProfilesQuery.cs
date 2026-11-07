using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Queries;

public record GetAllThresholdProfilesQuery(PatientId? PatientId = null);