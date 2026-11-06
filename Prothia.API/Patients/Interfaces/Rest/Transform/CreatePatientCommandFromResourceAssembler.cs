using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Patients.Interfaces.Rest.Resources;
using Prothia.API.Shared.Infrastructure.Interfaces.AspNetCore.Configuration.Extensions;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public static class CreatePatientCommandFromResourceAssembler
{
    public static CreatePatientCommand ToCommandFromResource(CreatePatientResource resource) =>
        new(new FullName(resource.FullName),
            new Dni(resource.Dni),
            resource.Age,
            EnumConversions.Parse<EAmputationLevel>(resource.Amputation, nameof(resource.Amputation)),
            EnumConversions.Parse<EKLevel>(resource.KLevel, nameof(resource.KLevel)),
            string.IsNullOrWhiteSpace(resource.ProsthesisCode) ? null : new ProsthesisCode(resource.ProsthesisCode));
}