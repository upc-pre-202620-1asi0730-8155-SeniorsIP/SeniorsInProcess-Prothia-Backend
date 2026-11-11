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
[Tags("Available Electronic Invoices Endpoints.")]
public class InvoicesController(
    IElectronicInvoiceCommandService electronicInvoiceCommandService,
    IElectronicInvoiceQueryService electronicInvoiceQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [EndpointSummary("Create Electronic Invoices")]
    [EndpointDescription("Issue a new electronic invoice.")]
    [ProducesResponseType(typeof(ElectronicInvoiceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateElectronicInvoiceResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateElectronicInvoiceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await electronicInvoiceCommandService.Handle(command, cancellationToken);

        return BillingActionResultAssembler.ToActionResultFromElectronicInvoiceResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdInvoice => CreatedAtAction(nameof(GetInvoiceById), new { id = createdInvoice.Id.Value },
                ElectronicInvoiceResourceFromEntityAssembler.ToResourceFromEntity(createdInvoice))
        );
    }

    [HttpGet]
    [EndpointSummary("Get Electronic Invoices")]
    [EndpointDescription("Get all electronic invoices.")]
    [ProducesResponseType(typeof(IEnumerable<ElectronicInvoiceResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvoices(CancellationToken cancellationToken = default)
    {
        var invoices = await electronicInvoiceQueryService.Handle(new GetAllElectronicInvoicesQuery(),
            cancellationToken);
        return Ok(invoices.Select(ElectronicInvoiceResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get Electronic Invoices by Id")]
    [EndpointDescription("Get an electronic invoice by its unique identifier.")]
    [ProducesResponseType(typeof(ElectronicInvoiceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoiceById(int id, CancellationToken cancellationToken = default)
    {
        var invoice = await electronicInvoiceQueryService.Handle(new GetElectronicInvoiceByIdQuery(id),
            cancellationToken);

        return BillingActionResultAssembler.ToActionResultFromGetElectronicInvoiceByIdResult(
            this,
            invoice,
            _errorLocalizer,
            _problemDetailsFactory,
            foundInvoice => Ok(ElectronicInvoiceResourceFromEntityAssembler.ToResourceFromEntity(foundInvoice))
        );
    }
}
