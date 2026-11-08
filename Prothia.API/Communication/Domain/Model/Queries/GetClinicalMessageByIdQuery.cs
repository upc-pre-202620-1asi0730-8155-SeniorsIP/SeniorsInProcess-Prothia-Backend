namespace Prothia.API.Communication.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a clinical message by its identifier.
/// </summary>
public record GetClinicalMessageByIdQuery(int Id);