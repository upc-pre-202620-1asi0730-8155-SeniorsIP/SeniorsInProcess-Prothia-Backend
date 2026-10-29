using Prothia.API.Patients.Domain.Model.ValueObjects;

namespace Prothia.API.Patients.Domain.Model.Queries;

public record GetPatientByDniQuery(Dni Dni);