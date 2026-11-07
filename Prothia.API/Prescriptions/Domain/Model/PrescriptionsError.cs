namespace Prothia.API.Prescriptions.Domain.Model;

public enum PrescriptionsError
{
    None,
    PatientNotFound,
    ExerciseNotFound,
    DuplicateExerciseName,
    PrescriptionNotFound,
    InvalidPrescriptionData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}