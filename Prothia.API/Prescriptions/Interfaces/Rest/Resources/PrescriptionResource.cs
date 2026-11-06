namespace Prothia.API.Prescriptions.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a patient's prescription plan.
/// </summary>
/// <param name="Id">The server-generated ID of the prescription</param>
/// <param name="PatientId">The patient the prescription belongs to</param>
/// <param name="PhaseNumber">Rehabilitation phase number (1-10)</param>
/// <param name="PhaseName">Rehabilitation phase name</param>
/// <param name="DurationWeeks">Plan duration in weeks (1-52)</param>
/// <param name="FrequencyPerWeek">Weekly frequency in days (1-7)</param>
/// <param name="Exercises">Prescribed exercises with dosage</param>
public record PrescriptionResource(
    int Id,
    int PatientId,
    int PhaseNumber,
    string PhaseName,
    int DurationWeeks,
    int FrequencyPerWeek,
    IReadOnlyList<PrescribedExerciseResource> Exercises);