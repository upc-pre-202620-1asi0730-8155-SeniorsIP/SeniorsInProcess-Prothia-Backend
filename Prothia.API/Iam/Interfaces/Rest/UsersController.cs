using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Prothia.API.Iam.Application.CommandServices;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Application.QueryServices;
using Prothia.API.Iam.Domain.Model;
using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Iam.Domain.Model.Errors;
using Prothia.API.Iam.Domain.Model.Queries;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Iam.Interfaces.Rest.Resources;
using Prothia.API.Iam.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;

namespace Prothia.API.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Tags("Available Users Endpoints.")]
[Authorize]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    ICurrentUserService currentUser,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    [EndpointSummary("Get current authenticated user profile")]
    [EndpointDescription("Returns the IAM account profile, role, and linked patientId of the currently authenticated user based on their JWT Bearer token. Authorized roles: ClinicalProfessional, Patient, Technician.")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!int.TryParse(rawUserId, out var userId) || userId <= 0)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status401Unauthorized,
                IamError.InvalidCredentials,
                "Authenticated user identifier claim is missing or invalid.");
        }

        var user = await userQueryService.Handle(new GetUserByIdQuery(new UserId(userId)), cancellationToken);
        if (user is not null && !user.IsActive)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status401Unauthorized,
                IamError.InactiveUser,
                IamErrors.InactiveUser.Message);
        }

        return IamActionResultAssembler.ToActionResultFromGetUserByIdResult(
            this,
            user,
            problemDetailsFactory,
            foundUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(foundUser)));
    }

    [HttpGet]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Get all managed user accounts")]
    [EndpointDescription("Lists user accounts belonging to the authenticated ClinicalProfessional's clinic (the clinician themselves plus Patient and Technician accounts they provisioned). Supports optional query filtering by role. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? role = null,
        CancellationToken cancellationToken = default)
    {
        ERole? roleFilter = null;
        if (Request.Query.ContainsKey(nameof(role)))
        {
            var trimmedRole = role?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedRole) ||
                int.TryParse(trimmedRole, out _) ||
                !Enum.TryParse<ERole>(trimmedRole, ignoreCase: true, out var parsedRole) ||
                !Enum.IsDefined(parsedRole))
            {
                return IamActionResultAssembler.ToProblemDetailsResult(
                    this,
                    problemDetailsFactory,
                    StatusCodes.Status400BadRequest,
                    IamError.InvalidUserData,
                    "Invalid role filter. Allowed values: ClinicalProfessional, Patient, Technician.");
            }

            roleFilter = parsedRole;
        }

        var users = await userQueryService.Handle(
            new GetAllUsersQuery(roleFilter, currentUser.UserId),
            cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Get a user account by ID")]
    [EndpointDescription("Retrieves a single IAM user account by its unique identifier, enforcing clinic ownership isolation. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status400BadRequest,
                IamError.InvalidUserData,
                "User id must be greater than zero.");
        }

        var user = await userQueryService.Handle(new GetUserByIdQuery(new UserId(id)), cancellationToken);
        if (user is not null &&
            user.Id.Value != currentUser.UserId &&
            user.CreatedByUserId != currentUser.UserId)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status403Forbidden,
                IamError.PrivilegeEscalationForbidden,
                "You can only access accounts registered under your clinic.");
        }

        return IamActionResultAssembler.ToActionResultFromGetUserByIdResult(
            this,
            user,
            problemDetailsFactory,
            foundUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(foundUser)));
    }

    [HttpPost]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Create a managed Patient or Technician account")]
    [EndpointDescription("Provisions a new Patient or Technician user account scoped to the authenticated ClinicalProfessional's clinic. Patient accounts can be linked to an existing clinical patientId. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateManagedUser(
        [FromBody] CreateManagedUserResource resource,
        CancellationToken cancellationToken = default)
    {
        var command = CreateManagedUserCommandFromResourceAssembler.ToCommandFromResource(resource) with
        {
            CreatedByUserId = currentUser.UserId
        };
        var result = await userCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromUserMutationResult(
            this,
            result,
            problemDetailsFactory,
            createdUser => CreatedAtAction(
                nameof(GetUserById),
                new { id = createdUser.Id.Value },
                UserResourceFromEntityAssembler.ToResourceFromEntity(createdUser)));
    }

    [HttpPatch("{id:int}/statuses")]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Update managed user account active status")]
    [EndpointDescription("Enables or disables a managed Patient or Technician account created by the authenticated ClinicalProfessional. Disabled accounts cannot sign in or use existing JWT tokens. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserStatus(
        int id,
        [FromBody] UpdateUserStatusResource resource,
        CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId is null)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status401Unauthorized,
                IamError.InvalidCredentials,
                "Authenticated user identifier claim is missing.");
        }

        var result = await userCommandService.Handle(
            new UpdateUserStatusCommand(id, resource.IsActive, currentUser.UserId.Value),
            cancellationToken);

        return IamActionResultAssembler.ToActionResultFromUserMutationResult(
            this,
            result,
            problemDetailsFactory,
            updatedUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(updatedUser)));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Delete a managed user account")]
    [EndpointDescription("Permanently deletes a managed Patient or Technician user account created by the authenticated ClinicalProfessional. Self-deletion and deletion of ClinicalProfessional accounts are forbidden. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteManagedUser(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId is null)
        {
            return IamActionResultAssembler.ToProblemDetailsResult(
                this,
                problemDetailsFactory,
                StatusCodes.Status401Unauthorized,
                IamError.InvalidCredentials,
                "Authenticated user identifier claim is missing.");
        }

        var result = await userCommandService.Handle(
            new DeleteManagedUserCommand(id, currentUser.UserId.Value),
            cancellationToken);

        return IamActionResultAssembler.ToActionResultFromDeleteResult(
            this,
            result,
            problemDetailsFactory,
            NoContent);
    }
}
