using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Interfaces.Rest.Resources;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public static class PatientResourceFromEntityAssembler
{
    public static PatientResource ToResourceFromEntity(Patient entity)
    {
        return new PatientResource(
            entity.Id.Value,
            entity.FullName.Value,
            entity.Dni.Value,
            entity.Age,
            entity.EAmputation.ToCamelCaseString(),
            entity.EkLevel.ToString(),
            entity.ProsthesisCode?.Value,
            entity.Adherence,
            entity.Symmetry,
            entity.Status.ToString().ToLowerInvariant());
    }
}