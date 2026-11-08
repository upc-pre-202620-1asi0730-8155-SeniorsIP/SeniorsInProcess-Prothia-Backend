using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Prescriptions.Domain.Model.Errors;

public static class PrescriptionsErrors
{
    public static readonly Error PatientNotFound =
        Error.NotFound("Prescriptions.PatientNotFound", "The specified patient does not exist.");

    public static readonly Error ExerciseNotFound =
        Error.NotFound("Prescriptions.ExerciseNotFound", "The specified exercise was not found.");

    public static readonly Error DuplicateExerciseName =
        Error.Conflict("Prescriptions.DuplicateExerciseName", "An exercise with this name already exists.");

    public static readonly Error PrescriptionNotFound =
        Error.NotFound("Prescriptions.PrescriptionNotFound", "The specified prescription was not found.");

    public static readonly Error InvalidPrescriptionData =
        Error.Validation("Prescriptions.InvalidPrescriptionData", "The prescription data provided is invalid.");
}