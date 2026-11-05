namespace Prothia.API.Patients.Domain.Model.Errors;

public class PatientsErrors
{
    public static readonly Error PatientNotFound =
        Error.NotFound("Patients.PatientNotFound", "The specified patient was not found.");

    public static readonly Error DniAlreadyRegistered =
        Error.Conflict("Patients.DniAlreadyRegistered", "A patient with this DNI is already registered.");

    public static readonly Error InvalidPatientData =
        Error.Validation("Patients.InvalidPatientData", "The patient data provided is invalid.");

    public static readonly Error PatientCreationFailed =
        Error.Failure("Patients.PatientCreationFailed", "An error occurred while creating the patient.");
}