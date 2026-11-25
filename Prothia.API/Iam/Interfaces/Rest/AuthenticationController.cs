using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Application.CommandServices;
using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Iam.Interfaces.Rest.Resources;
using Prothia.API.Iam.Interfaces.Rest.Transform;
using Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Authentication")]
public sealed class AuthenticationController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("sign-up")]
    [EndpointSummary("Register a user account")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp(
        [FromBody] SignUpResource resource,
        CancellationToken cancellationToken)
    {
        var result = await userCommandService.Handle(
            SignUpCommandFromResourceAssembler.ToCommandFromResource(resource),
            cancellationToken);

        return IamActionResultAssembler.ToActionResult(
            this,
            result,
            problemDetailsFactory,
            user => CreatedAtAction(
                nameof(UsersController.GetUserById),
                "Users",
                new { id = user.Id.Value },
                UserResourceFromEntityAssembler.ToResourceFromEntity(user)));
    }

    [HttpPost("sign-in")]
    [EndpointSummary("Sign in and issue a bearer token")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInResource resource,
        CancellationToken cancellationToken)
    {
        var result = await userCommandService.Handle(
            SignInCommandFromResourceAssembler.ToCommandFromResource(resource),
            cancellationToken);

        return IamActionResultAssembler.ToActionResult(
            this,
            result,
            problemDetailsFactory,
            authenticatedUser => Ok(
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser)));
    }
}
