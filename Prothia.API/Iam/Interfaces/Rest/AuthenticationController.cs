using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Application.CommandServices;
using Prothia.API.Iam.Interfaces.Rest.Resources;
using Prothia.API.Iam.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Available Authentication Endpoints.")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [EndpointSummary("Register a new ClinicalProfessional account")]
    [EndpointDescription("Public self-registration endpoint for creating a new ClinicalProfessional account with an isolated clinic workspace. Patient and Technician accounts must be provisioned by a clinician via POST /api/v1/users. Public endpoint (AllowAnonymous).")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp(
        [FromBody] SignUpResource resource,
        CancellationToken cancellationToken = default)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromUserMutationResult(
            this,
            result,
            problemDetailsFactory,
            createdUser => CreatedAtAction(
                nameof(UsersController.GetUserById),
                "Users",
                new { id = createdUser.Id.Value },
                UserResourceFromEntityAssembler.ToResourceFromEntity(createdUser)));
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    [EndpointSummary("Authenticate user and issue JWT token")]
    [EndpointDescription("Validates username and password credentials for an active user account and returns the user profile along with a signed JWT Bearer token. Public endpoint (AllowAnonymous).")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInResource resource,
        CancellationToken cancellationToken = default)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromSignInResult(
            this,
            result,
            problemDetailsFactory,
            auth => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(auth.user, auth.token)));
    }
}
