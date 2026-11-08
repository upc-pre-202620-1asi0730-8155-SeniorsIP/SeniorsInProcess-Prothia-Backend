using Prothia.API.Communication.Domain.Model.Commands;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;

namespace Prothia.API.Communication.Domain.Model.Aggregates;

/// <summary>
///     Clinical data transmission shared between a rehabilitation clinic and an orthoprosthetic workshop for a patient.
/// </summary>
public partial class ClinicalTransmission : AggregateRoot
{
    public const int PatientNameMaxLength = 150;
    public const int PatientDniMaxLength = 20;
    public const int TechnicalNotesMaxLength = 2000;
    public const int StatusLabelMaxLength = 100;
    public const int SummaryMaxLength = 500;
    public const int CreatedAtDateMaxLength = 50;

    public ClinicalTransmissionId Id { get; private set; }
    public PatientId PatientId { get; private set; }
    public string PatientName { get; private set; }
    public string PatientDni { get; private set; }
    public WorkshopName WorkshopName { get; private set; }
    public IReadOnlyList<string> SharedModules { get; private set; }
    public string TechnicalNotes { get; private set; }
    public TransmissionStatus Status { get; private set; }
    public string StatusLabel { get; private set; }
    public string Summary { get; private set; }
    public string CreatedAtDate { get; private set; }

    protected ClinicalTransmission()
    {
        PatientName = string.Empty;
        PatientDni = string.Empty;
        WorkshopName = null!;
        SharedModules = [];
        TechnicalNotes = string.Empty;
        StatusLabel = string.Empty;
        Summary = string.Empty;
        CreatedAtDate = string.Empty;
    }

    public ClinicalTransmission(CreateClinicalTransmissionCommand command, string patientName)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (string.IsNullOrWhiteSpace(patientName))
            throw new ArgumentException("PatientName cannot be null or whitespace.", nameof(patientName));
        if (patientName.Trim().Length > PatientNameMaxLength)
            throw new ArgumentException($"PatientName cannot exceed {PatientNameMaxLength} characters.",
                nameof(patientName));
        if (string.IsNullOrWhiteSpace(command.PatientDni))
            throw new ArgumentException("PatientDni cannot be null or whitespace.", nameof(command));
        if (command.PatientDni.Trim().Length > PatientDniMaxLength)
            throw new ArgumentException($"PatientDni cannot exceed {PatientDniMaxLength} characters.", nameof(command));
        if (command.SharedModules is null)
            throw new ArgumentException("SharedModules cannot be null.", nameof(command));

        var cleanedModules = command.SharedModules
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .Select(m => m.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (cleanedModules.Count == 0)
            throw new ArgumentException("At least one shared module is required.", nameof(command));

        var notes = command.TechnicalNotes?.Trim() ?? string.Empty;
        if (notes.Length > TechnicalNotesMaxLength)
            throw new ArgumentException($"TechnicalNotes cannot exceed {TechnicalNotesMaxLength} characters.",
                nameof(command));

        var summary = string.IsNullOrWhiteSpace(command.Summary)
            ? $"{cleanedModules.Count} módulos clínicos compartidos"
            : command.Summary.Trim();
        if (summary.Length > SummaryMaxLength)
            throw new ArgumentException($"Summary cannot exceed {SummaryMaxLength} characters.", nameof(command));

        var createdAtDate = string.IsNullOrWhiteSpace(command.CreatedAtDate)
            ? DateTimeOffset.UtcNow.ToString("yyyy-MM-dd")
            : command.CreatedAtDate.Trim();
        if (createdAtDate.Length > CreatedAtDateMaxLength)
            throw new ArgumentException($"CreatedAtDate cannot exceed {CreatedAtDateMaxLength} characters.",
                nameof(command));

        PatientId = command.PatientId;
        PatientName = patientName.Trim();
        PatientDni = command.PatientDni.Trim();
        WorkshopName = command.WorkshopName;
        SharedModules = cleanedModules.AsReadOnly();
        TechnicalNotes = notes;
        Status = TransmissionStatus.Pending;
        StatusLabel = "Pendiente de Recepción";
        Summary = summary;
        CreatedAtDate = createdAtDate;
    }
}
