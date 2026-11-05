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
    
    public EAmputationLevel EAmputation {get; private set;}
    
    public EKLevel EkLevel { get; private set; }
    
    public ProsthesisCode? ProsthesisCode { get; private set; }
    
    public int Adherence { get; private set; }
    
    public int Symmetry {get; private set;}
    
    public EPatientStatus Status {get; private set;}

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
        if (!Enum.IsDefined(command.EAmputation))
            throw new ArgumentOutOfRangeException(nameof(command), command.EAmputation, "Invalid amputation level.");
        if (!Enum.IsDefined(command.EkLevel))
            throw new ArgumentOutOfRangeException(nameof(command), command.EkLevel, "Invalid K level.");

        FullName = command.FullName;
        Dni = command.Dni;
        Age = command.Age;
        EAmputation = command.EAmputation;
        EkLevel = command.EkLevel;
        ProsthesisCode = command.ProsthesisCode;
        Adherence = 0;
        Symmetry = 0;
        Status = EPatientStatus.Stable;
    }
}