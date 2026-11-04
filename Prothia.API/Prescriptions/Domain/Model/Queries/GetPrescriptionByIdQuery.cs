namespace Prothia.API.Prescriptions.Domain.Model.Queries;
/// <summary>
/// Query to retrieve a prescription by its identifier.
/// </summary>
public record GetPrescriptionByIdQuery(int Id);