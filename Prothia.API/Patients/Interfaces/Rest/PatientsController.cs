using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Patients.Application.CommandServices;
using Prothia.API.Patients.Application.QueryServices;
using Prothia.API.Patients.Domain.Model.Queries;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.Rest.Resources;
using Prothia.API.Patients.Interfaces.Rest.Transform;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Patients.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Patient Endpoints.")]
public class PatientsController(
    IPatientCommandService patientCommandService,
    IPatientQueryService patientQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;
 
    [HttpPost]
    [EndpointSummary("Create Patient")]
    [EndpointDescription("Register a new amputee patient in the PROTHIA clinical platform.")]
    [ProducesResponseType(typeof(PatientResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreatePatientCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await patientCommandService.Handle(command, cancellationToken);
 
        return PatientsActionResultAssembler.ToActionResultFromCreatePatientResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdPatient => CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.Id },
                PatientResourceFromEntityAssembler.ToResourceFromEntity(createdPatient))
        );
    }
 
    [HttpGet]
    [EndpointSummary("Get Patients")]
    [EndpointDescription("Get all patients or filter by DNI.")]
    [ProducesResponseType(typeof(IEnumerable<PatientResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatients([FromQuery] string? dni = null,
        CancellationToken cancellationToken = default)
    {
        if (!Request.Query.ContainsKey(nameof(dni)))
        {
            var patients = await patientQueryService.Handle(new GetAllPatientsQuery(), cancellationToken);
            return Ok(patients.Select(PatientResourceFromEntityAssembler.ToResourceFromEntity));
        }
 
        var patient = await patientQueryService.Handle(new GetPatientByDniQuery(new Dni(dni ?? string.Empty)),
            cancellationToken);
        return PatientsActionResultAssembler.ToActionResultFromGetPatientByIdResult(
            this,
            patient,
            _errorLocalizer,
            _problemDetailsFactory,
            foundPatient => Ok(PatientResourceFromEntityAssembler.ToResourceFromEntity(foundPatient))
        );
    }
 
    [HttpGet("{id:int}")]
    [EndpointSummary("Get Patient by Id")]
    [EndpointDescription("Get a patient by its unique identifier.")]
    [ProducesResponseType(typeof(PatientResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientById(int id, CancellationToken cancellationToken = default)
    {
        var patient = await patientQueryService.Handle(new GetPatientByIdQuery(id), cancellationToken);
 
        return PatientsActionResultAssembler.ToActionResultFromGetPatientByIdResult(
            this,
            patient,
            _errorLocalizer,
            _problemDetailsFactory,
            foundPatient => Ok(PatientResourceFromEntityAssembler.ToResourceFromEntity(foundPatient))
        );
    }
}
