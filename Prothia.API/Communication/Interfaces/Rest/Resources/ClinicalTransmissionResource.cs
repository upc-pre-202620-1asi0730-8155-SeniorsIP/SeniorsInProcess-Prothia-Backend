namespace Prothia.API.Communication.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a clinical data transmission.
/// </summary>
public record ClinicalTransmissionResource(
    int Id,
    int PatientId,
    string PatientName,
    string PatientDni,
    string WorkshopName,
    IReadOnlyList<string> SharedModules,
    string TechnicalNotes,
    string Status,
    string StatusLabel,
    string Summary,
    string CreatedAtDate);