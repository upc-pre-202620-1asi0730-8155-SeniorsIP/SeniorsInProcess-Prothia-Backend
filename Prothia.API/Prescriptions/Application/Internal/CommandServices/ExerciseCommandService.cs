using Microsoft.EntityFrameworkCore;
using Prothia.API.Prescriptions.Application.Errors;
using Prothia.API.Prescriptions.Application.Services;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Commands;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Shared.Application.Extensions;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Prescriptions.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling exercise catalog commands.
/// </summary>
/// <param name="exerciseRepository">Repository for accessing exercise data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class ExerciseCommandService(
    IExerciseRepository exerciseRepository,
    IUnitOfWork unitOfWork,
    ILogger<ExerciseCommandService> logger)
    : IExerciseCommandService
{
    /// <inheritdoc />
    public async Task<Result<Exercise, ECreateExerciseError>> Handle(CreateExerciseCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existing = await exerciseRepository.FindByNameAsync(command.Name, cancellationToken);
            if (existing is not null)
            {
                logger.LogWarning("Duplicate exercise rejected for name {ExerciseName}", command.Name);
                return new Result<Exercise, ECreateExerciseError>.Failure(ECreateExerciseError.DuplicateExercise);
            }

            var exercise = new Exercise(command);
            await exerciseRepository.AddAsync(exercise, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Exercise, ECreateExerciseError>.Success(exercise);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating exercise {ExerciseName}", command.Name);
            return new Result<Exercise, ECreateExerciseError>.Failure(ECreateExerciseError.InvalidExercise);
        }
        catch (DbUpdateException ex) when (ex.IsDuplicateKeyViolation())
        {
            logger.LogWarning(ex, "Duplicate key violation creating exercise {ExerciseName}", command.Name);
            return new Result<Exercise, ECreateExerciseError>.Failure(ECreateExerciseError.DuplicateExercise);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating exercise {ExerciseName}", command.Name);
            return new Result<Exercise, ECreateExerciseError>.Failure(ECreateExerciseError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating exercise {ExerciseName}", command.Name);
            return new Result<Exercise, ECreateExerciseError>.Failure(ECreateExerciseError.UnexpectedError);
        }
    }
}
