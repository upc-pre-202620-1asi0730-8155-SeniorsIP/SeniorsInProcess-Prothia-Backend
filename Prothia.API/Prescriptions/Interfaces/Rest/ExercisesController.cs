using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Application.QueryServices;
using Prothia.API.Prescriptions.Domain.Model.Queries;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;
using Prothia.API.Prescriptions.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Prescriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Exercise Catalog Endpoints.")]
public class ExercisesController(
    IExerciseCommandService exerciseCommandService,
    IExerciseQueryService exerciseQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Exercise")]
    [EndpointDescription("Register an exercise in the clinical catalog.")]
    [ProducesResponseType(typeof(ExerciseResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateExerciseCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await exerciseCommandService.Handle(command, cancellationToken);

        return PrescriptionsActionResultAssembler.ToActionResultFromExerciseResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdExercise => CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id.Value },
                ExerciseResourceFromEntityAssembler.ToResourceFromEntity(createdExercise))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Exercises")]
    [EndpointDescription("Get all exercises in the catalog.")]
    [ProducesResponseType(typeof(IEnumerable<ExerciseResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExercises(CancellationToken cancellationToken = default)
    {
        var exercises = await exerciseQueryService.Handle(new GetAllExercisesQuery(), cancellationToken);
        return Ok(exercises.Select(ExerciseResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Exercise by Id")]
    [EndpointDescription("Get an exercise by its unique identifier.")]
    [ProducesResponseType(typeof(ExerciseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExerciseById(int id, CancellationToken cancellationToken = default)
    {
        var exercise = await exerciseQueryService.Handle(new GetExerciseByIdQuery(id), cancellationToken);

        return PrescriptionsActionResultAssembler.ToActionResultFromGetExerciseByIdResult(
            this,
            exercise,
            _errorLocalizer,
            _problemDetailsFactory,
            foundExercise => Ok(ExerciseResourceFromEntityAssembler.ToResourceFromEntity(foundExercise))
        );
    }
}
