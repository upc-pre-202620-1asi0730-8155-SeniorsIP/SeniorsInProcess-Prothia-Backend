using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.Rest.Resources;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public static class CreatePatientCommandFromResourceAssembler
{
    public static CreatePatientCommand ToCommandFromResource(CreatePatientResource resource) =>
        new(new FullName(resource.FullName),
            new Dni(resource.Dni),
            resource.Age,
            Enum.Parse<EAmputationLevel>(resource.Amputation, true),
            Enum.Parse<EKLevel>(resource.KLevel, true),
            string.IsNullOrWhiteSpace(resource.ProsthesisCode) ? null : new ProsthesisCode(resource.ProsthesisCode));
}