using Prothia.API.Patients.Domain.Model.ValueObjects;

namespace Prothia.API.Patients.Domain.Model.Commands;

public record CreatePatientCommand(
    FullName FullName,
    Dni Dni,
    int Age,
    EAmputationLevel EAmputation,
    EKLevel EkLevel,
    ProsthesisCode? ProsthesisCode);
