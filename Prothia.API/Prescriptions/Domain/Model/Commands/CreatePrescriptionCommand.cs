using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Commands;

/// <summary>
///     Command to create a rehabilitation prescription for a patient.
/// </summary>
public record CreatePrescriptionCommand(
    PatientId PatientId,
    int PhaseNumber,
    PhaseName PhaseName,
    int DurationWeeks,
    int FrequencyPerWeek,
    IReadOnlyList<PrescribedExerciseItem> Exercises);