using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Billing.Application.CommandServices;
using Prothia.API.Billing.Application.QueryServices;
using Prothia.API.Billing.Domain.Model.Queries;
using Prothia.API.Billing.Interfaces.Rest.Resources;
using Prothia.API.Billing.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Billing.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Clinic Subscriptions Endpoints.")]
public class ClinicSubscriptionsController(
    IClinicSubscriptionCommandService clinicSubscriptionCommandService,
    IClinicSubscriptionQueryService clinicSubscriptionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Clinic Subscriptions")]
    [EndpointDescription("Create a new institutional clinic subscription.")]
    [ProducesResponseType(typeof(ClinicSubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClinicSubscription([FromBody] CreateClinicSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateClinicSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await clinicSubscriptionCommandService.Handle(command, cancellationToken);

        return BillingActionResultAssembler.ToActionResultFromClinicSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdSubscription => CreatedAtAction(nameof(GetClinicSubscriptionById),
                new { id = createdSubscription.Id.Value },
                ClinicSubscriptionResourceFromEntityAssembler.ToResourceFromEntity(createdSubscription))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Clinic Subscriptions")]
    [EndpointDescription("Get all institutional clinic subscriptions.")]
    [ProducesResponseType(typeof(IEnumerable<ClinicSubscriptionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClinicSubscriptions(CancellationToken cancellationToken = default)
    {
        var subscriptions = await clinicSubscriptionQueryService.Handle(new GetAllClinicSubscriptionsQuery(),
            cancellationToken);
        return Ok(subscriptions.Select(ClinicSubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Clinic Subscriptions by Id")]
    [EndpointDescription("Get a clinic subscription by its unique identifier.")]
    [ProducesResponseType(typeof(ClinicSubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClinicSubscriptionById(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await clinicSubscriptionQueryService.Handle(new GetClinicSubscriptionByIdQuery(id),
            cancellationToken);

        return BillingActionResultAssembler.ToActionResultFromGetClinicSubscriptionByIdResult(
            this,
            subscription,
            _errorLocalizer,
            _problemDetailsFactory,
            foundSubscription => Ok(ClinicSubscriptionResourceFromEntityAssembler.ToResourceFromEntity(foundSubscription))
        );
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update Clinic Subscriptions")]
    [EndpointDescription("Update an existing institutional clinic subscription.")]
    [ProducesResponseType(typeof(ClinicSubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClinicSubscription(int id,
        [FromBody] UpdateClinicSubscriptionResource resource, CancellationToken cancellationToken = default)
    {
        var command = UpdateClinicSubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await clinicSubscriptionCommandService.Handle(command, cancellationToken);

        return BillingActionResultAssembler.ToActionResultFromClinicSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedSubscription => Ok(ClinicSubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updatedSubscription))
        );
    }
}
