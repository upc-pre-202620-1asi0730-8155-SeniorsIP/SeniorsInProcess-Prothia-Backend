namespace Prothia.API.Patients.Interfaces.Rest.Resources;

public record PatientResource(
    int Id,
    string FullName,
    string Dni,
    int Age,
    string Amputation,
    string KLevel,
    string? ProsthesisCode,
    int Adherence,
    int Symmetry,
    string Status);