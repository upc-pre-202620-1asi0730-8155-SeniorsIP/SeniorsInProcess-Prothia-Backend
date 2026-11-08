using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all clinical transmissions, optionally filtered by patient.
/// </summary>
public record GetAllClinicalTransmissionsQuery(PatientId? PatientId = null);