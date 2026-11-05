using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Shared.Domain.Model.Aggregates.Internal;

namespace Prothia.API.Prescriptions.Domain.Model.Aggregates;

/// <summary>
///     Rehabilitation exercise registered in the clinical catalog.
/// </summary>
public partial class Exercise : AggregateRoot
{
    public const int FocusMaxLength = 300;
    public const int FocusTagMaxLength = 60;

    public ExerciseId Id { get; }
    public ExerciseName Name { get; private set; }
    public IReadOnlyList<string> Focus { get; private set; }

    protected Exercise()
    {
        Name = null!;
        Focus = [];
    }

    public Exercise(CreateExerciseCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.Focus is null)
            throw new ArgumentException("Focus cannot be null.", nameof(command));

        var cleanedFocus = command.Focus
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (cleanedFocus.Count == 0)
            throw new ArgumentException("At least one focus tag is required.", nameof(command));
        if (cleanedFocus.Any(tag => tag.Length > FocusTagMaxLength))
            throw new ArgumentException($"Each focus tag cannot exceed {FocusTagMaxLength} characters.", nameof(command));

        Name = command.Name;
        Focus = cleanedFocus.AsReadOnly();
    }
}