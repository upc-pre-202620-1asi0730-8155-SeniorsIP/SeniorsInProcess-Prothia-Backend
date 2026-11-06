using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Application.QueryServices;
using Prothia.API.Prescriptions.Domain.Model.Queries;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;
using Prothia.API.Prescriptions.Interfaces.Rest.Transform;

using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Prescriptions.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Available Prescription Endpoints.")]
public class PrescriptionsController(
    IPrescriptionCommandService prescriptionCommandService,
    IPrescriptionQueryService prescriptionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Prescription")]
    [EndpointDescription("Create a rehabilitation prescription for a patient.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreatePrescriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await prescriptionCommandService.Handle(command, cancellationToken);

        return PrescriptionsActionResultAssembler.ToActionResultFromPrescriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdPrescription => CreatedAtAction(nameof(GetPrescriptionById), new { id = createdPrescription.Id.Value },
                PrescriptionResourceFromEntityAssembler.ToResourceFromEntity(createdPrescription))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Prescriptions")]
    [EndpointDescription("Get prescriptions optionally filtered by patientId.")]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPrescriptions([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllPrescriptionsQuery(patientId is null ? null : new PatientId(patientId.Value));
        var prescriptions = await prescriptionQueryService.Handle(query, cancellationToken);
        return Ok(prescriptions.Select(PrescriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Prescription by Id")]
    [EndpointDescription("Get a prescription by its unique identifier.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescriptionById(int id, CancellationToken cancellationToken = default)
    {
        var prescription = await prescriptionQueryService.Handle(new GetPrescriptionByIdQuery(id), cancellationToken);

        return PrescriptionsActionResultAssembler.ToActionResultFromGetPrescriptionByIdResult(
            this,
            prescription,
            _errorLocalizer,
            _problemDetailsFactory,
            foundPrescription => Ok(PrescriptionResourceFromEntityAssembler.ToResourceFromEntity(foundPrescription))
        );
    }

    [HttpPut("{id:int}")]
    [EndpointSummary("Update Prescription")]
    [EndpointDescription("Update an existing rehabilitation prescription.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription(int id,
        [FromBody] UpdatePrescriptionResource resource, CancellationToken cancellationToken = default)
    {
        var command = UpdatePrescriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await prescriptionCommandService.Handle(command, cancellationToken);

        return PrescriptionsActionResultAssembler.ToActionResultFromPrescriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updatedPrescription => Ok(PrescriptionResourceFromEntityAssembler.ToResourceFromEntity(updatedPrescription))
        );
    }
}
