using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a PrescriptionResource from a Prescription.
/// </summary>
public static class PrescriptionResourceFromEntityAssembler
{
    public static PrescriptionResource ToResourceFromEntity(Prescription entity) =>
        new(entity.Id.Value,
            entity.PatientId.Value,
            entity.PhaseNumber,
            entity.PhaseName.Value,
            entity.DurationWeeks,
            entity.FrequencyPerWeek,
            entity.Exercises
                .Select(e => new PrescribedExerciseResource(e.ExerciseId.Value, e.Name, e.Sets, e.Reps, e.RestSeconds))
                .ToList());
}