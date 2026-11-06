using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdatePrescriptionCommand from an UpdatePrescriptionResource.
/// </summary>
public static class UpdatePrescriptionCommandFromResourceAssembler
{
    public static UpdatePrescriptionCommand ToCommandFromResource(int prescriptionId,
        UpdatePrescriptionResource resource) =>
        new(prescriptionId,
            resource.PhaseNumber,
            new PhaseName(resource.PhaseName),
            resource.DurationWeeks,
            resource.FrequencyPerWeek,
            (resource.Exercises ?? [])
            .Select(e => new PrescribedExerciseItem(e.ExerciseId, e.Name, e.Sets, e.Reps, e.RestSeconds))
            .ToList());
}