using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Patients.Application.Services;
using Prothia.API.Patients.Domain.Model.Queries;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.Rest.Resources;
using Prothia.API.Patients.Interfaces.Rest.Transform;

namespace Prothia.API.Patients.Interfaces.Rest;

public class PatientsController(
   IPatientCommandService patientCommandService,
    IPatientQueryService patientQueryService,
    IStringLocalizer<ErrorMessages> localizer,
    ILogger<PatientsController> logger)
    : ControllerBase
{
    
    [HttpPost]
    [ProducesResponseType(typeof(PatientResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreatePatient([FromBody] CreatePatientResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createPatientCommand = CreatePatientCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await patientCommandService.Handle(createPatientCommand, cancellationToken);
            return ActionResultFromCreatePatientResultAssembler.ToActionResultFromCreatePatientResult(
                result,
                this,
                localizer,
                nameof(GetPatientById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating patient for Dni {Dni}",
                Dni.MaskedForLogging(resource.Dni));
            return BadRequest(localizer["InvalidPatientRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating patient for Dni {Dni}",
                Dni.MaskedForLogging(resource.Dni));
            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingPatient"].Value,
                statusCode: 500);
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPatients([FromQuery] string? dni = null,
        CancellationToken cancellationToken = default)
    {
        if (!Request.Query.ContainsKey(nameof(dni)))
        {
            var patients = await patientQueryService.Handle(new GetAllPatientsQuery(), cancellationToken);
            return Ok(patients.Select(PatientResourceFromEntityAssembler.ToResourceFromEntity));
        }

        try
        {
            var patient = await patientQueryService.Handle(new GetPatientByDniQuery(new Dni(dni ?? string.Empty)),
                cancellationToken);
            if (patient is null) return NotFound();
            return Ok(PatientResourceFromEntityAssembler.ToResourceFromEntity(patient));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while querying patient for Dni {Dni}",
                Dni.MaskedForLogging(dni));
            return BadRequest(localizer["InvalidPatientQuery"].Value);
        }
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PatientResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetPatientById(int id, CancellationToken cancellationToken = default)
    {
        var result = await patientQueryService.Handle(new GetPatientByIdQuery(id), cancellationToken);
        if (result is null) return NotFound();
        return Ok(PatientResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
}
