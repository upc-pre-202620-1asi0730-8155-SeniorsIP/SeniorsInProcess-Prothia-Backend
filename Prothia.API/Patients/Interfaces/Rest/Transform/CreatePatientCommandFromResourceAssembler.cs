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
            ParseAmputationLevel(resource.Amputation),
            EnumConversions.Parse<EKLevel>(resource.KLevel, nameof(resource.KLevel)),
            string.IsNullOrWhiteSpace(resource.ProsthesisCode) ? null : new ProsthesisCode(resource.ProsthesisCode));

    private static EAmputationLevel ParseAmputationLevel(string? value)
    {
        return value?.Trim() switch
        {
            "Transtibial Der." => EAmputationLevel.TranstibialRight,
            "Transtibial Izq." => EAmputationLevel.TranstibialLeft,
            "Transtibial Bilateral" => EAmputationLevel.TranstibialBilateral,
            "Transfemoral Der." => EAmputationLevel.TransfemoralRight,
            "Transfemoral Izq." => EAmputationLevel.TransfemoralLeft,
            "Transfemoral Bilateral" => EAmputationLevel.TransfemoralBilateral,
            _ => EnumConversions.Parse<EAmputationLevel>(value?.Replace(" ", string.Empty), nameof(CreatePatientResource.Amputation))
        };
    }
}
