using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Domain.Repositories;

public interface IProsthesisRepository : IBaseRepository<Prosthesis>
{
    Task<Prosthesis?> FindBySerialNumberAsync(SerialNumber serialNumber,
        CancellationToken cancellationToken = default);
}