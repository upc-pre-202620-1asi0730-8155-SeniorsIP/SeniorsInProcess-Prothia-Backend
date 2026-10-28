namespace Prothia.API.Prescriptions.Domain.Model.ValueObjects;

/// <summary>
/// Represents a rehabilitation phase in a therapeutic plan
/// </summary>
public sealed record RehabilitationPhase
{
    private const int MinPhaseNumber = 1;
    private const int MaxPhaseNumber = 10;
    private const int MaxNameLength = 50;
    
    public int Number { get; }
    public string Name { get; }
    
    public static readonly RehabilitationPhase Initial = new (1, "Initial");
    public static readonly RehabilitationPhase Adjustment = new (2, "Adjustment");
    public static readonly RehabilitationPhase Consolidation = new(3, "Consolidation");
    public static readonly RehabilitationPhase Advanced = new (4, "Advanced");

    public RehabilitationPhase(int number, string name)
    {
        if (number is <MinPhaseNumber or > MaxPhaseNumber)
            throw new ArgumentOutOfRangeException(nameof(number), number, $"Phase number must be between {MinPhaseNumber} and {MaxPhaseNumber}.");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Phase name cannot be null or whitespace.", nameof(name));
        
        var trimmedName = name.Trim();
        if (trimmedName.Length > MaxNameLength)
            throw new ArgumentException($"Phase name cannot exceed {MaxNameLength} characters.", nameof(name));
        
        Number = number;
        Name = trimmedName;
        
    }
    public override string ToString() => $"Fase {Number} - {Name}";
}