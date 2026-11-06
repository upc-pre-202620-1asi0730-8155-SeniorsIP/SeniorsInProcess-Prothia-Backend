using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Billing.Application.CommandServices;

public interface IElectronicInvoiceCommandService
{
    Task<Result<ElectronicInvoice>> Handle(CreateElectronicInvoiceCommand command,
        CancellationToken cancellationToken = default);
}