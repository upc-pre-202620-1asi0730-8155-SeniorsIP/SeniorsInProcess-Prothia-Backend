namespace Prothia.API.Patients.Domain.Model;

public enum PatientsError
{
    None,
    PatientNotFound,
    DniAlreadyRegistered,
    InvalidPatientData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}