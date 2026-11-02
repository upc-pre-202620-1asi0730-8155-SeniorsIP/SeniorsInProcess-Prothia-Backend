using Prothia.API.Billing.Application.Errors;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Billing.Application.Services;

public interface IElectronicInvoiceCommandService
{
    Task<Result<ElectronicInvoice, ECreateElectronicInvoiceError>> Handle(CreateElectronicInvoiceCommand command,
        CancellationToken cancellationToken = default);
}