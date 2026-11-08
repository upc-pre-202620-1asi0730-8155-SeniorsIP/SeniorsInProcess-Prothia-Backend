using Microsoft.EntityFrameworkCore;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.ValueObjects;
using Prothia.API.Workshop.Domain.Repositories;

namespace Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProsthesisRepository(AppDbContext context) : BaseRepository<Prosthesis>(context), IProsthesisRepository
{
    public async Task<Prosthesis?> FindBySerialNumberAsync(SerialNumber serialNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Prosthesis>()
            .FirstOrDefaultAsync(p => p.SerialNumber == serialNumber, cancellationToken);
    }
}