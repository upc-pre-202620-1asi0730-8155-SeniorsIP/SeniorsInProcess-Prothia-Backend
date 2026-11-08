using Prothia.API.Communication.Domain.Model.ValueObjects;

namespace Prothia.API.Communication.Domain.Model.Commands;

/// <summary>
///     Command to create a clinical data transmission to an orthoprosthetic workshop.
/// </summary>
public record CreateClinicalTransmissionCommand(
    PatientId PatientId,
    string PatientDni,
    WorkshopName WorkshopName,
    IReadOnlyList<string> SharedModules,
    string TechnicalNotes,
    string Summary,
    string CreatedAtDate);