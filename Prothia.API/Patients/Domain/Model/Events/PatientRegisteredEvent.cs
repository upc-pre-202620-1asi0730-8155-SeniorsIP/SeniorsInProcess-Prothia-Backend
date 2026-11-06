using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Patients.Domain.Model.Events;

public sealed record PatientRegisteredEvent(string FullName, string MaskedDni, EKLevel EkLevel) : IEvent;
