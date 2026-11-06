using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Events;

namespace Prothia.API.Prescriptions.Domain.Model.Events;

public record PrescriptionCreatedEvent(
    PrescriptionId PrescriptionId,
    PatientId PatientId,
    int PhaseNumber,
    string PhaseName) : IEvent;