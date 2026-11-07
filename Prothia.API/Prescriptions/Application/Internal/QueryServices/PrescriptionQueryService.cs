using Prothia.API.Prescriptions.Application.QueryServices;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Queries;
using Prothia.API.Prescriptions.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.QueryServices;

public class PrescriptionQueryService(IPrescriptionRepository prescriptionRepository) : IPrescriptionQueryService
{
    public async Task<IEnumerable<Prescription>> Handle(GetAllPrescriptionsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.PatientId is null)
            return await prescriptionRepository.ListAsNoTrackingAsync(cancellationToken);

        return await prescriptionRepository.ListByPatientIdAsync(query.PatientId.Value, cancellationToken);
    }

    public async Task<Prescription?> Handle(GetPrescriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await prescriptionRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);
    }
}