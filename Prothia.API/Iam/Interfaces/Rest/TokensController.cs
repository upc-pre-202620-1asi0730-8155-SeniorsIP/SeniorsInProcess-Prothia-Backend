using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Application.CommandServices;
using Prothia.API.Iam.Interfaces.Rest.Resources;
using Prothia.API.Iam.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Available Tokens Endpoints.")]
public class TokensController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Create a JWT authentication token")]
    [EndpointDescription("Authenticates active user credentials and issues a signed JWT Bearer access token for authorizing subsequent API requests. Public endpoint (AllowAnonymous).")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateToken(
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
