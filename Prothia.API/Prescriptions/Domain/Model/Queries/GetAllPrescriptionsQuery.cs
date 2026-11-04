using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Queries;
/// <summary>
///     Query to retrieve prescriptions, optionally filtered by patient.
/// </summary>
public record GetAllPrescriptionsQuery(PatientId? PatientId = null);