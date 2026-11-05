using System.ComponentModel.DataAnnotations;
using Prothia.API.Patients.Domain.Model.Aggregates;

namespace Prothia.API.Patients.Interfaces.Rest.Resources;

public record CreatePatientResource(
    [Required(ErrorMessage = "RequiredFullName")]
    string FullName,
    [Required(ErrorMessage = "RequiredDni")]
    string Dni,
    [Range(Patient.MinAge, Patient.MaxAge, ErrorMessage = "InvalidAge")]
    int Age,
    [Required(ErrorMessage = "RequiredAmputation")]
    string Amputation,
    [Required(ErrorMessage = "RequiredKLevel")]
    string KLevel,
    string? ProsthesisCode = null);