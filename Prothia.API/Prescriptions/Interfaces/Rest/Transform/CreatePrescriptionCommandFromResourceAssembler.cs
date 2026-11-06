using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreatePrescriptionCommand from a CreatePrescriptionResource.
/// </summary>
public static class CreatePrescriptionCommandFromResourceAssembler
{
    public static CreatePrescriptionCommand ToCommandFromResource(CreatePrescriptionResource resource) =>
        new(new PatientId(resource.PatientId),
            resource.PhaseNumber,
            new PhaseName(resource.PhaseName),
            resource.DurationWeeks,
            resource.FrequencyPerWeek,
            (resource.Exercises ?? [])
            .Select(e => new PrescribedExerciseItem(e.ExerciseId, e.Name, e.Sets, e.Reps, e.RestSeconds))
            .ToList());
}