using Prothia.API.Patients.Domain.Model.Commands;
using Prothia.API.Patients.Domain.Model.ValueObjects;

namespace Prothia.API.Patients.Domain.Model.Aggregates;

public partial class Patient
{
    public const int MinAge = 1;
    public const int MaxAge = 90;
    
    public int Id { get; private set; }
    
    public FullName FullName { get; private set; }
    
    public Dni Dni { get; private set; }
    
    public int Age { get; private set; }
    
    public AmputationLevel Amputation {get; private set;}
    
    public KLevel KLevel { get; private set; }
    
    public ProsthesisCode? ProsthesisCode { get; private set; }
    
    public int Adherence { get; private set; }
    
    public int Symmetry {get; private set;}
    
    public PatientStatus Status {get; private set;}

    protected Patient()
    {
        FullName = null!;
        Dni = null!;
    }

    public Patient(CreatePatientCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.Age is < MinAge or > MaxAge)
            throw new ArgumentOutOfRangeException(nameof(command), command.Age,
        $"Age must be between {MinAge} and {MaxAge}.");
        if (!Enum.IsDefined(command.Amputation))
            throw new ArgumentOutOfRangeException(nameof(command), command.Amputation, "Invalid amputation level.");
        if (!Enum.IsDefined(command.KLevel))
            throw new ArgumentOutOfRangeException(nameof(command), command.KLevel, "Invalid K level.");

        FullName = command.FullName;
        Dni = command.Dni;
        Age = command.Age;
        Amputation = command.Amputation;
        KLevel = command.KLevel;
        ProsthesisCode = command.ProsthesisCode;
        Adherence = 0;
        Symmetry = 0;
        Status = PatientStatus.Stable;
    }
}