using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Iam.Domain.Model.ValueObjects;
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
[Authorize]
[Tags("Available Exercises Endpoints.")]
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
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Create a clinical rehabilitation exercise")]
    [EndpointDescription("Registers a new therapeutic exercise and its biomechanical focus tags in the clinical exercise library catalog. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(ExerciseResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    [Authorize(Roles = $"{nameof(ERole.ClinicalProfessional)},{nameof(ERole.Patient)}")]
    [EndpointSummary("Get all exercises in the clinical library")]
    [EndpointDescription("Retrieves the catalog of available rehabilitation exercises and their biomechanical focus tags for building patient prescriptions. Authorized roles: ClinicalProfessional, Patient.")]
    [ProducesResponseType(typeof(IEnumerable<ExerciseResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetExercises(CancellationToken cancellationToken = default)
    {
        var exercises = await exerciseQueryService.Handle(new GetAllExercisesQuery(), cancellationToken);
        return Ok(exercises.Select(ExerciseResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{nameof(ERole.ClinicalProfessional)},{nameof(ERole.Patient)}")]
    [EndpointSummary("Get an exercise by ID")]
    [EndpointDescription("Retrieves a single exercise definition from the clinical library by its unique identifier. Authorized roles: ClinicalProfessional, Patient.")]
    [ProducesResponseType(typeof(ExerciseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExerciseById(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                "Exercise id must be greater than zero.");
        }

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
