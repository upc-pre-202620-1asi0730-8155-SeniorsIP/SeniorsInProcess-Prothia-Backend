using Prothia.API.Prescriptions.Domain.Model.ValueObjects;

namespace Prothia.API.Prescriptions.Domain.Model.Queries;

public record GetExerciseByIdQuery(ExerciseId Id)
{
    public GetExerciseByIdQuery(int id) : this(new ExerciseId(id))
    {
    }
}