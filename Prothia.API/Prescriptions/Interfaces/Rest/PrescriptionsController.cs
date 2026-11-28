using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Application.Internal.OutboundServices.ACL;
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
[Authorize]
[Tags("Available Prescriptions Endpoints.")]
public class PrescriptionsController(
    IPrescriptionCommandService prescriptionCommandService,
    IPrescriptionQueryService prescriptionQueryService,
    ExternalPatientService externalPatientService,
    ICurrentUserService currentUser,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Create a rehabilitation prescription")]
    [EndpointDescription("Creates a personalized rehabilitation exercise plan (phase, duration in weeks, weekly frequency, and prescribed exercises with sets, reps, and rest times) for a patient in the clinician's clinic. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionResource resource,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not null)
        {
            var ownerId = await externalPatientService.FetchClinicalProfessionalIdByPatientIdAsync(
                resource.PatientId,
                cancellationToken);
            if (ownerId.HasValue && ownerId.Value != currentUser.UserId.Value)
            {
                return _problemDetailsFactory.CreateStatusProblemDetails(
                    this,
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    "You can only create prescriptions for patients registered under your clinic.");
            }
        }

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
    [Authorize(Roles = $"{nameof(ERole.ClinicalProfessional)},{nameof(ERole.Patient)}")]
    [EndpointSummary("Get all rehabilitation prescriptions")]
    [EndpointDescription("Retrieves rehabilitation prescriptions scoped to the authenticated clinic or patient, optionally filtered by the patientId query parameter. Authorized roles: ClinicalProfessional, Patient.")]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPrescriptions([FromQuery] int? patientId = null,
        CancellationToken cancellationToken = default)
    {
        if (patientId is <= 0)
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                "patientId must be greater than zero.");
        }

        int? effectivePatientId = patientId;
        IReadOnlySet<int>? allowedPatientIds = null;
        if (currentUser.IsPatient)
        {
            if (patientId.HasValue)
            {
                if (currentUser.PatientId is null || patientId.Value != currentUser.PatientId.Value)
                {
                    return _problemDetailsFactory.CreateStatusProblemDetails(
                        this,
                        StatusCodes.Status403Forbidden,
                        "Forbidden",
                        "Patients can only access their own prescriptions.");
                }
            }
            else if (currentUser.PatientId is null)
            {
                return Ok(Array.Empty<PrescriptionResource>());
            }

            effectivePatientId = currentUser.PatientId;
        }
        else if (currentUser.UserId is not null)
        {
            allowedPatientIds = await externalPatientService.FetchPatientIdsByClinicalProfessionalIdAsync(
                currentUser.UserId.Value,
                cancellationToken);
            if (patientId.HasValue && !allowedPatientIds.Contains(patientId.Value))
            {
                return _problemDetailsFactory.CreateStatusProblemDetails(
                    this,
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    "You can only access prescriptions for patients registered under your clinic.");
            }
        }
        else
        {
            return Ok(Array.Empty<PrescriptionResource>());
        }

        var query = new GetAllPrescriptionsQuery(effectivePatientId is null ? null : new PatientId(effectivePatientId.Value));
        var prescriptions = await prescriptionQueryService.Handle(query, cancellationToken);
        if (allowedPatientIds is not null)
            prescriptions = prescriptions.Where(p => allowedPatientIds.Contains(p.PatientId.Value));
        return Ok(prescriptions.Select(PrescriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{nameof(ERole.ClinicalProfessional)},{nameof(ERole.Patient)}")]
    [EndpointSummary("Get a rehabilitation prescription by ID")]
    [EndpointDescription("Retrieves a specific rehabilitation prescription and its prescribed exercise routine by unique identifier. Authorized roles: ClinicalProfessional, Patient.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescriptionById(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                "Prescription id must be greater than zero.");
        }

        if (currentUser.IsPatient && currentUser.PatientId is null)
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status403Forbidden,
                "Forbidden",
                "Patients without a linked patient record cannot access prescriptions.");
        }

        var prescription = await prescriptionQueryService.Handle(new GetPrescriptionByIdQuery(id), cancellationToken);

        if (currentUser.IsPatient && (prescription is null || prescription.PatientId.Value != currentUser.PatientId!.Value))
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status403Forbidden,
                "Forbidden",
                "Patients can only access their own prescriptions.");
        }

        if (!currentUser.IsPatient && prescription is not null)
        {
            var ownerId = await externalPatientService.FetchClinicalProfessionalIdByPatientIdAsync(
                prescription.PatientId.Value,
                cancellationToken);
            if (currentUser.UserId is null || ownerId != currentUser.UserId.Value)
            {
                return _problemDetailsFactory.CreateStatusProblemDetails(
                    this,
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    "You can only access prescriptions for patients registered under your clinic.");
            }
        }

        return PrescriptionsActionResultAssembler.ToActionResultFromGetPrescriptionByIdResult(
            this,
            prescription,
            _errorLocalizer,
            _problemDetailsFactory,
            foundPrescription => Ok(PrescriptionResourceFromEntityAssembler.ToResourceFromEntity(foundPrescription))
        );
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = nameof(ERole.ClinicalProfessional))]
    [EndpointSummary("Update an existing rehabilitation prescription")]
    [EndpointDescription("Updates the phase, schedule parameters, and prescribed exercise list of an existing patient rehabilitation prescription. Authorized role: ClinicalProfessional.")]
    [ProducesResponseType(typeof(PrescriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription(int id,
        [FromBody] UpdatePrescriptionResource resource, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return _problemDetailsFactory.CreateStatusProblemDetails(
                this,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                "Prescription id must be greater than zero.");
        }

        var existing = await prescriptionQueryService.Handle(new GetPrescriptionByIdQuery(id), cancellationToken);
        if (existing is not null)
        {
            var ownerId = await externalPatientService.FetchClinicalProfessionalIdByPatientIdAsync(
                existing.PatientId.Value,
                cancellationToken);
            if (currentUser.UserId is null || ownerId != currentUser.UserId.Value)
            {
                return _problemDetailsFactory.CreateStatusProblemDetails(
                    this,
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    "You can only update prescriptions for patients registered under your clinic.");
            }
        }

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
