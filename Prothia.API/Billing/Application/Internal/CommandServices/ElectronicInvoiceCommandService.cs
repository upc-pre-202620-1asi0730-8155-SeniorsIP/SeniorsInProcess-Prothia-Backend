using Prothia.API.Billing.Application.CommandServices;
using Prothia.API.Billing.Domain.Model;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.Errors;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Billing.Application.Internal.CommandServices;

public class ElectronicInvoiceCommandService(
    IElectronicInvoiceRepository electronicInvoiceRepository,
    IUnitOfWork unitOfWork)
    : IElectronicInvoiceCommandService
{
    public async Task<Result<ElectronicInvoice>> Handle(
        CreateElectronicInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var existing =
            await electronicInvoiceRepository.FindByInvoiceNumberAsync(command.InvoiceNumber, cancellationToken);
        if (existing is not null)
            return Result<ElectronicInvoice>.Failure(BillingErrors.DuplicateInvoiceNumber,
                BillingError.DuplicateInvoiceNumber);

        var invoice = new ElectronicInvoice(command);
        await electronicInvoiceRepository.AddAsync(invoice, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ElectronicInvoice>.Success(invoice);
    }
}