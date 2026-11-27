using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Domain.Model;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.Errors;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
    private static IActionResult AsProblemJson(IActionResult actionResult)
    {
        if (actionResult is ObjectResult objectResult)
        {
            objectResult.ContentTypes.Clear();
            objectResult.ContentTypes.Add(MediaTypeNames.Application.ProblemJson);
        }

        return actionResult;
    }

    public static IActionResult ToProblemDetailsResult(
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory,
        int statusCode,
        IamError error,
        string detail)
    {
        return AsProblemJson(problemDetailsFactory.CreateProblemDetails(controller, statusCode, error, detail));
    }

    private static int ToStatusCodeFromIamError(IamError error) =>
        error switch
        {
            IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
            IamError.InactiveUser => StatusCodes.Status401Unauthorized,
            IamError.PrivilegeEscalationForbidden => StatusCodes.Status403Forbidden,
            IamError.UserNotFound => StatusCodes.Status404NotFound,
            IamError.PatientNotFound => StatusCodes.Status404NotFound,
            IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.EmailAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.OperationCancelled => StatusCodes.Status409Conflict,
            IamError.InvalidUserData => StatusCodes.Status400BadRequest,
            IamError.DatabaseError => StatusCodes.Status500InternalServerError,
            IamError.InternalServerError => StatusCodes.Status500InternalServerError,
            IamError.ExternalServiceError => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status400BadRequest
        };

    public static IActionResult ToActionResultFromSignInResult(
        ControllerBase controller,
        Result<(User user, string token)> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<(User user, string token), IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value);

        var iamError = result.Error is IamError err ? err : IamError.InvalidCredentials;
        var statusCode = ToStatusCodeFromIamError(iamError);
        return AsProblemJson(problemDetailsFactory.CreateProblemDetails(controller, statusCode, iamError, result.Message));
    }

    public static IActionResult ToActionResultFromUserMutationResult(
        ControllerBase controller,
        Result<User> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var iamError = result.Error is IamError err ? err : IamError.InvalidUserData;
        var statusCode = ToStatusCodeFromIamError(iamError);
        return AsProblemJson(problemDetailsFactory.CreateProblemDetails(controller, statusCode, iamError, result.Message));
    }

    public static IActionResult ToActionResultFromGetUserByIdResult(
        ControllerBase controller,
        User? user,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (user is null)
        {
            return AsProblemJson(problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromIamError(IamError.UserNotFound),
                IamError.UserNotFound,
                IamErrors.UserNotFound.Message));
        }

        return successAction(user);
    }

    public static IActionResult ToActionResultFromDeleteResult(
        ControllerBase controller,
        Result<bool> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();

        var iamError = result.Error is IamError err ? err : IamError.InvalidUserData;
        var statusCode = ToStatusCodeFromIamError(iamError);
        return AsProblemJson(problemDetailsFactory.CreateProblemDetails(controller, statusCode, iamError, result.Message));
    }
}
