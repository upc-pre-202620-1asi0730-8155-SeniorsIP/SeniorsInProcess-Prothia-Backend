using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Billing.Domain.Repositories;

public interface IElectronicInvoiceRepository : IBaseRepository<ElectronicInvoice>
{
    Task<ElectronicInvoice?> FindByInvoiceNumberAsync(InvoiceNumber invoiceNumber,
        CancellationToken cancellationToken = default);
}