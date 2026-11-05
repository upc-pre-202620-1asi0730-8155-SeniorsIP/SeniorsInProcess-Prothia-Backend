using Prothia.API.Analytics.Domain.Model.ValueObjects;

namespace Prothia.API.Analytics.Domain.Model.Queries;

public record GetAllClinicalReportsQuery(PatientId? PatientId = null);