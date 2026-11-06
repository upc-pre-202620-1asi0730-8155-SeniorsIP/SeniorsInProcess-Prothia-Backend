using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Queries;

namespace Prothia.API.Billing.Application.QueryServices;

public interface IElectronicInvoiceQueryService
{
    Task<IEnumerable<ElectronicInvoice>> Handle(GetAllElectronicInvoicesQuery query,
        CancellationToken cancellationToken = default);

    Task<ElectronicInvoice?> Handle(GetElectronicInvoiceByIdQuery query, CancellationToken cancellationToken = default);
}