namespace Prothia.API.Communication.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a clinical transmission by its identifier.
/// </summary>
public record GetClinicalTransmissionByIdQuery(int Id);