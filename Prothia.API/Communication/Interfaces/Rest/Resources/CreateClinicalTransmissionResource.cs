using System.ComponentModel.DataAnnotations;

namespace Prothia.API.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a clinical data transmission.
/// </summary>
public record CreateClinicalTransmissionResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidPatientId")]
    int PatientId,
    [Required(ErrorMessage = "RequiredPatientDni")]
    string PatientDni,
    [Required(ErrorMessage = "RequiredWorkshopName")]
    string WorkshopName,
    [Required(ErrorMessage = "RequiredSharedModules")]
    IReadOnlyList<string> SharedModules,
    string? TechnicalNotes = null,
    string? Summary = null,
    string? CreatedAtDate = null);