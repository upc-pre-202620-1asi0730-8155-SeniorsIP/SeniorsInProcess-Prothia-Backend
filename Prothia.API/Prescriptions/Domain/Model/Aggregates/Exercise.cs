using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

/// <summary>
///     Rehabilitation exercise in the clinical catalog.
/// </summary>
public partial class Exercise
{
    public int Id { get; private set;  }
    
    public ExerciseName Name { get; private set;  }
    public IReadOnlyList<string> Focus { get; private set;  }

    protected Exercise()
    {
        Name = null!;
        Focus = [];
    }

    public Exercise(CreateExerciseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.Focus is null || command.Focus.Count == 0)
            throw new ArgumentException("Focus cannot be empty.", nameof(command));
        var cleanedFocus = command.Focus
            .Where(f => !string.IsNullOrWhiteSpace(f))
            .Select(f => f.Trim())
            .ToList();
        if (cleanedFocus.Count == 0)
            throw new ArgumentException("Focus must contain at least one non-empty item.", nameof(command));

        Name = command.Name;
        Focus = cleanedFocus;
    }
    
}