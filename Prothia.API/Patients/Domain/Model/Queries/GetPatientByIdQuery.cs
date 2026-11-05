using Prothia.API.Patients.Domain.Model.ValueObjects;

namespace Prothia.API.Patients.Domain.Model.Queries;

public record GetPatientByIdQuery(PatientId Id) 
{
    public GetPatientByIdQuery(int id) : this(new PatientId(id))
    {
        
    }
};