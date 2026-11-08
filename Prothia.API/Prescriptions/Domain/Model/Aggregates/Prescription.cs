using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.Entities;
using Prothia.API.Prescriptions.Domain.Model.Events;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

/// <summary>
///     Patient rehabilitation prescription plan aggregate root.
/// </summary>
public partial class Prescription : AggregateRoot
{
    public const int PhaseNumberMin = 1;
    public const int PhaseNumberMax = 10;
    public const int DurationWeeksMin = 1;
    public const int DurationWeeksMax = 52;
    public const int FrequencyMin = 1;
    public const int FrequencyMax = 7;

    private readonly List<PrescribedExercise> _exercises = [];

    public PrescriptionId Id { get; }
    public PatientId PatientId { get; private set; }
    public int PhaseNumber { get; private set; }
    public PhaseName PhaseName { get; private set; }
    public int DurationWeeks { get; private set; }
    public int FrequencyPerWeek { get; private set; }
    public IReadOnlyList<PrescribedExercise> Exercises => _exercises.AsReadOnly();

    protected Prescription()
    {
        PhaseName = null!;
    }

    public Prescription(CreatePrescriptionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        PatientId = command.PatientId;
        PhaseName = null!;
        ApplyPlan(command.PhaseNumber, command.PhaseName, command.DurationWeeks, command.FrequencyPerWeek,
            command.Exercises);

        AddDomainEvent(new PrescriptionCreatedEvent(Id, PatientId, PhaseNumber, PhaseName.Value));
    }

    public void UpdatePlan(
        int phaseNumber,
        PhaseName phaseName,
        int durationWeeks,
        int frequencyPerWeek,
        IReadOnlyList<PrescribedExerciseItem> exercises)
    {
        ApplyPlan(phaseNumber, phaseName, durationWeeks, frequencyPerWeek, exercises);
    }

    private void ApplyPlan(
        int phaseNumber,
        PhaseName phaseName,
        int durationWeeks,
        int frequencyPerWeek,
        IReadOnlyList<PrescribedExerciseItem> exercises)
    {
        if (phaseNumber is < PhaseNumberMin or > PhaseNumberMax)
            throw new ArgumentOutOfRangeException(nameof(phaseNumber), phaseNumber,
                $"PhaseNumber must be between {PhaseNumberMin} and {PhaseNumberMax}.");
        if (durationWeeks is < DurationWeeksMin or > DurationWeeksMax)
            throw new ArgumentOutOfRangeException(nameof(durationWeeks), durationWeeks,
                $"DurationWeeks must be between {DurationWeeksMin} and {DurationWeeksMax}.");
        if (frequencyPerWeek is < FrequencyMin or > FrequencyMax)
            throw new ArgumentOutOfRangeException(nameof(frequencyPerWeek), frequencyPerWeek,
                $"FrequencyPerWeek must be between {FrequencyMin} and {FrequencyMax}.");
        if (exercises is null || exercises.Count == 0)
            throw new ArgumentException("At least one prescribed exercise is required.", nameof(exercises));

        PhaseNumber = phaseNumber;
        PhaseName = phaseName ?? throw new ArgumentNullException(nameof(phaseName));
        DurationWeeks = durationWeeks;
        FrequencyPerWeek = frequencyPerWeek;

        _exercises.Clear();
        foreach (var item in exercises)
        {
            _exercises.Add(new PrescribedExercise(item));
        }
    }
}
