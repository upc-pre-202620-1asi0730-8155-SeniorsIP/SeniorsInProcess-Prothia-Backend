namespace Prothia.API.Iam.Domain.Model;

public enum IamError
{
    UserNotFound,
    UsernameAlreadyTaken,
    InvalidCredentials,
    InvalidRegistrationData,
    DatabaseError,
    PatientAccountAlreadyExists
}