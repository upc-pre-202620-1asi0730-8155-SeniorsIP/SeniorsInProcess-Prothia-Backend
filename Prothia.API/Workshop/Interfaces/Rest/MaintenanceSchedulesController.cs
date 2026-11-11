using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Model.Queries;
using Prothia.API.Workshop.Interfaces.Rest.Resources;
using Prothia.API.Workshop.Interfaces.Rest.Transform;

namespace Prothia.API.Workshop.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Maintenance Schedules Endpoints.")]
public class MaintenanceSchedulesController(
    IMaintenanceScheduleCommandService maintenanceScheduleCommandService,
    IMaintenanceScheduleQueryService maintenanceScheduleQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Maintenance Schedules")]
    [EndpointDescription("Schedule a maintenance intervention in the workshop.")]
    [ProducesResponseType(typeof(MaintenanceScheduleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMaintenanceSchedule([FromBody] CreateMaintenanceScheduleResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateMaintenanceScheduleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await maintenanceScheduleCommandService.Handle(command, cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromMaintenanceScheduleResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdSchedule => CreatedAtAction(nameof(GetMaintenanceScheduleById),
                new { id = createdSchedule.Id.Value },
                MaintenanceScheduleResourceFromEntityAssembler.ToResourceFromEntity(createdSchedule))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Maintenance Schedules")]
    [EndpointDescription("Get all maintenance schedules in the workshop.")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceScheduleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMaintenanceSchedules(CancellationToken cancellationToken = default)
    {
        var schedules = await maintenanceScheduleQueryService.Handle(new GetAllMaintenanceSchedulesQuery(),
            cancellationToken);
        return Ok(schedules.Select(MaintenanceScheduleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Maintenance Schedules by Id")]
    [EndpointDescription("Get a maintenance schedule by its unique identifier.")]
    [ProducesResponseType(typeof(MaintenanceScheduleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMaintenanceScheduleById(int id, CancellationToken cancellationToken = default)
    {
        var schedule = await maintenanceScheduleQueryService.Handle(new GetMaintenanceScheduleByIdQuery(id),
            cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromGetMaintenanceScheduleByIdResult(
            this,
            schedule,
            _errorLocalizer,
            _problemDetailsFactory,
            foundSchedule => Ok(MaintenanceScheduleResourceFromEntityAssembler.ToResourceFromEntity(foundSchedule))
        );
    }

    [HttpPut("{id:int}/complete")]
    [EndpointSummary("Complete Maintenance Schedules")]
    [EndpointDescription("Mark a scheduled maintenance intervention as completed.")]
    [ProducesResponseType(typeof(MaintenanceScheduleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteMaintenanceSchedule(int id,
        [FromBody] CompleteMaintenanceResource? resource = null,
        CancellationToken cancellationToken = default)
    {
        var result = await maintenanceScheduleCommandService.CompleteAsync(id, resource?.Notes, cancellationToken);

        return WorkshopActionResultAssembler.ToActionResultFromMaintenanceScheduleResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedSchedule => Ok(MaintenanceScheduleResourceFromEntityAssembler.ToResourceFromEntity(updatedSchedule))
        );
    }
}

public record CompleteMaintenanceResource(string? Notes = null);
