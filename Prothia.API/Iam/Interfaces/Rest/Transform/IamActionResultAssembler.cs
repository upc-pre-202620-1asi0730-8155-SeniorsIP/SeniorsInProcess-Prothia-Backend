using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Domain.Model;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);

        var statusCode = result.Error switch
        {
            IamError.UserNotFound => StatusCodes.Status404NotFound,
            IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
            IamError.InvalidRegistrationData => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            statusCode,
            (Enum?)null,
            result.Message);
    }
}
