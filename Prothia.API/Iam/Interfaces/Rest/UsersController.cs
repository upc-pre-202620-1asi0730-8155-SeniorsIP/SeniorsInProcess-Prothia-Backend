using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Prothia.API.Iam.Application.QueryServices;
using Prothia.API.Iam.Domain.Model.Queries;
using Prothia.API.Iam.Interfaces.Rest.Resources;
using Prothia.API.Iam.Interfaces.Rest.Transform;
using Prothia.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace Prothia.API.Iam.Interfaces.Rest;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Users")]
public sealed class UsersController(IUserQueryService userQueryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("List registered users")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserResource>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery(), cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a registered user by ID")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResource>> GetUserById(int id, CancellationToken cancellationToken)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);
        return user is null
            ? NotFound()
            : Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}
