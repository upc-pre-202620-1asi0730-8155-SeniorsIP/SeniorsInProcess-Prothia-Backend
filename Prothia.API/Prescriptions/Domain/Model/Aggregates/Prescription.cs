using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.Entities;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

/// <summary>
///     Therapeutic exercise prescription plan assigned to a patient for a rehabilitation phase.
/// </summary>
public partial class Prescription
{
    public const int PhaseNumberMin = 1;
    public const int PhaseNumberMax = 10;
    public const int DurationWeeksMin = 1;
    public const int DurationWeeksMax = 52;
    public const int FrequencyMin = 1;
    public const int FrequencyMax = 7;
    
    public int Id { get; private set; }
    public PatientId PatientId { get; private set; }
    public int PhaseNumber { get; private set; }
    public PhaseName PhaseName { get; private set; }
    public int DurationWeeks { get; private set; }
    public int FrequencyPerWeek{ get; private set; }
    public ICollection<PrescribedExercise> Excercise { get; private set; }

    protected Prescription()
    {
        PatientId = null!;
        PhaseName = null!;
        Excercise = new List<PrescribedExercise>();
    }

    public Prescription(CreatePrescriptionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        PatientId = command.PatientId;
        PhaseName = null!;
        Exercises = new List<PrescribedExercise>();
        ApplyPlan(command.PhaseNumber, command.PhaseName, command.DurationWeeks, command.FrequencyPerWeek,
            command.Exercises);
    }

    /// <summary>
    ///     Replaces the rehabilitation phase, schedule parameters, and prescribed exercises of the prescription.
    /// </summary>

    public void Update(UpdatePrescriptionCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ApplyPlan(command.PhaseNumber, command.PhaseName, command.DurationWeeks, command.FrequencyPerWeek,
            command.Exercises);
    }

    private void ApplyPlan(int phaseNumber, PhaseName phaseName, int durationWeeks, int frequencyPerWeek,
        IReadOnlyList<PrescribedExerciseItem> exercises)
    {
        ArgumentNullException.ThrowIfNull(phaseName);
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
            throw new ArgumentException("Exercises cannot be empty.", nameof(exercises));

        var mappedExercises = exercises.Select(e => new PrescribedExercise(e)).ToList();

        PhaseNumber = phaseNumber;
        PhaseName = phaseName;
        DurationWeeks = durationWeeks;
        FrequencyPerWeek = frequencyPerWeek;
        Exercises.Clear();
        foreach (var exercise in mappedExercises)
            Exercises.Add(exercise);
    }

}