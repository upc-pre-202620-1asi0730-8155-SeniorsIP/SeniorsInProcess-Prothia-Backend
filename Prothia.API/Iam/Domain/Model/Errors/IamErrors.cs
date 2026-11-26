using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound("Iam.UserNotFound", "User not found.");

    public static readonly Error UsernameAlreadyTaken =
        Error.Conflict("Iam.UsernameAlreadyTaken", "That username is already taken.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Iam.InvalidCredentials", "Invalid username or password.");

    public static readonly Error InvalidRegistrationData =
        Error.Validation("Iam.InvalidRegistrationData", "The registration details are invalid.");

    public static readonly Error DatabaseError =
        Error.Failure("Iam.DatabaseError", "An error occurred while saving the user.");

    public static readonly Error PatientAccountAlreadyExists =
        Error.Conflict("Iam.PatientAccountAlreadyExists", "This patient already has an account.");
}
