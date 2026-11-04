using Prothia.API.Patients.Domain.Model.ValueObjects;

namespace Prothia.API.Patients.Domain.Model.Events;

public sealed record PatientRegisteredEvent(string FullName, string MaskedDni, KLevel KLevel) : IEvent;
