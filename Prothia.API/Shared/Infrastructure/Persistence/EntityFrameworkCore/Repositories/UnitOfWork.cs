using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}