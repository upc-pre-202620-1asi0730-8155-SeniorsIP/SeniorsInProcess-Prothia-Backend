using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Commands;

/// <summary>
///     Command to update an existing rehabilitation prescription.
/// </summary>
public record UpdatePrescriptionCommand(
    int PrescriptionId,
    int PhaseNumber,
    PhaseName PhaseName,
    int DurationWeeks,
    int FrequencyPerWeek,
    IReadOnlyList<PrescribedExerciseItem> Exercises);