using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ElectronicInvoiceRepository(AppDbContext context)
    : BaseRepository<ElectronicInvoice>(context), IElectronicInvoiceRepository
{
    public new async Task<IEnumerable<ElectronicInvoice>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<ElectronicInvoice>()
            .OrderByDescending(i => i.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<ElectronicInvoice?> FindByInvoiceNumberAsync(InvoiceNumber invoiceNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<ElectronicInvoice>()
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
    }
}