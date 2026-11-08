using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Queries;

namespace Prothia.API.Prescriptions.Application.QueryServices;

public interface IPrescriptionQueryService
{
    Task<IEnumerable<Prescription>> Handle(GetAllPrescriptionsQuery query,
        CancellationToken cancellationToken = default);
    Task<Prescription?> Handle(GetPrescriptionByIdQuery query, CancellationToken cancellationToken = default);
}