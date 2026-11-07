using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Queries;

public record GetPrescriptionByIdQuery(PrescriptionId Id)
{
    public GetPrescriptionByIdQuery(int id) : this(new PrescriptionId(id))
    {
    }
}