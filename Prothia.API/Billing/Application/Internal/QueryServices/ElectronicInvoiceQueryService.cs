using Prothia.API.Billing.Application.QueryServices;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Queries;
using Prothia.API.Billing.Domain.Repositories;

namespace Prothia.API.Billing.Application.Internal.QueryServices;

public class ElectronicInvoiceQueryService(IElectronicInvoiceRepository electronicInvoiceRepository)
    : IElectronicInvoiceQueryService
{
    public async Task<IEnumerable<ElectronicInvoice>> Handle(GetAllElectronicInvoicesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await electronicInvoiceRepository.ListAsNoTrackingAsync(cancellationToken);
    }

    public async Task<ElectronicInvoice?> Handle(GetElectronicInvoiceByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await electronicInvoiceRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}