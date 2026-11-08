using System.ComponentModel.DataAnnotations;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a rehabilitation prescription.
/// </summary>
/// <param name="PatientId">The patient the prescription belongs to (required)</param>
/// <param name="PhaseNumber">Rehabilitation phase number (1-10)</param>
/// <param name="PhaseName">Rehabilitation phase name (required)</param>
/// <param name="DurationWeeks">Plan duration in weeks (1-52)</param>
/// <param name="FrequencyPerWeek">Weekly frequency in days (1-7)</param>
/// <param name="Exercises">Prescribed exercises with dosage (at least one required)</param>
public record CreatePrescriptionResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidPatientId")]
    int PatientId,
    [Range(Prescription.PhaseNumberMin, Prescription.PhaseNumberMax, ErrorMessage = "InvalidPhaseNumber")]
    int PhaseNumber,
    [Required(ErrorMessage = "RequiredPhaseName")]
    string PhaseName,
    [Range(Prescription.DurationWeeksMin, Prescription.DurationWeeksMax, ErrorMessage = "InvalidDurationWeeks")]
    int DurationWeeks,
    [Range(Prescription.FrequencyMin, Prescription.FrequencyMax, ErrorMessage = "InvalidFrequencyPerWeek")]
    int FrequencyPerWeek,
    [Required(ErrorMessage = "RequiredPrescriptionExercises")]
    [MinLength(1, ErrorMessage = "RequiredPrescriptionExercises")]
    IReadOnlyList<PrescribedExerciseResource> Exercises);