using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Interfaces.Rest.Resources;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public static class PatientResourceFromEntityAssembler
{
    public static PatientResource ToResourceFromEntity(Patient entity) =>
        new(entity.Id,
            entity.FullName.Value,
            entity.Dni.Value,
            entity.Age,
            entity.Amputation.ToString(),
            entity.KLevel.ToString(),
            entity.ProsthesisCode?.Value,
            entity.Adherence,
            entity.Symmetry,
            entity.Status.ToString().ToLowerInvariant());
}