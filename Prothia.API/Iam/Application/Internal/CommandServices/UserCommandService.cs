using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Prothia.API.Iam.Application.CommandServices;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Domain.Model;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Iam.Domain.Model.Errors;
using Prothia.API.Iam.Domain.Repositories;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Iam.Application.Internal.CommandServices;

public sealed partial class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork) : IUserCommandService
{
    private const int MinimumPasswordLength = 12;
    private const int MaximumPasswordBytes = 72;

    [GeneratedRegex("^[a-zA-Z0-9._-]{3,50}$", RegexOptions.CultureInvariant)]
    private static partial Regex UsernamePattern();

    public async Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var username = command.Username?.Trim().ToLowerInvariant();
        var password = command.Password;

        if (username is null ||
            !UsernamePattern().IsMatch(username) ||
            !IsValidPassword(password) ||
            !TryGetPublicRegistrationRole(command.Role, out var role))
        {
            return Result<User>.Failure(IamErrors.InvalidRegistrationData, IamError.InvalidRegistrationData);
        }

        if (await userRepository.ExistsByUsernameAsync(username, cancellationToken))
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken, IamError.UsernameAlreadyTaken);

        return await PersistUserAsync(username, password, role, null, cancellationToken);
    }

    public async Task<Result<User>> CreatePatientAccountAsync(
        int patientId,
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        if (patientId <= 0 ||
            !UsernamePattern().IsMatch(normalizedUsername) ||
            !IsValidPassword(password))
        {
            return Result<User>.Failure(IamErrors.InvalidRegistrationData, IamError.InvalidRegistrationData);
        }

        if (await userRepository.ExistsByPatientIdAsync(patientId, cancellationToken))
            return Result<User>.Failure(
                IamErrors.PatientAccountAlreadyExists,
                IamError.PatientAccountAlreadyExists);

        if (await userRepository.ExistsByUsernameAsync(normalizedUsername, cancellationToken))
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken, IamError.UsernameAlreadyTaken);

        return await PersistUserAsync(
            normalizedUsername,
            password,
            ERole.Patient,
            patientId,
            cancellationToken);
    }

    private async Task<Result<User>> PersistUserAsync(
        string username,
        string password,
        ERole role,
        int? patientId,
        CancellationToken cancellationToken)
    {
        var user = new User(username, hashingService.HashPassword(password), role, patientId);
        await userRepository.AddAsync(user, cancellationToken);

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.GetBaseException() is MySqlException { Number: 1062 })
        {
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken, IamError.UsernameAlreadyTaken);
        }

        return Result<User>.Success(user);
    }

    private static bool IsValidPassword(string? password) =>
        password is { Length: >= MinimumPasswordLength } &&
        Encoding.UTF8.GetByteCount(password) <= MaximumPasswordBytes;

    private static bool TryGetPublicRegistrationRole(string? value, out ERole role)
    {
        if (string.Equals(value, nameof(ERole.Physiotherapist), StringComparison.OrdinalIgnoreCase))
        {
            role = ERole.Physiotherapist;
            return true;
        }

        if (string.Equals(value, nameof(ERole.Technician), StringComparison.OrdinalIgnoreCase))
        {
            role = ERole.Technician;
            return true;
        }

        role = default;
        return false;
    }

    public async Task<Result<AuthenticatedUser>> Handle(
        SignInCommand command,
        CancellationToken cancellationToken = default)
    {
        var username = command.Username?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(command.Password))
            return Result<AuthenticatedUser>.Failure(IamErrors.InvalidCredentials, IamError.InvalidCredentials);

        var user = await userRepository.FindByUsernameAsync(username, cancellationToken);
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<AuthenticatedUser>.Failure(IamErrors.InvalidCredentials, IamError.InvalidCredentials);

        var issuedToken = tokenService.GenerateToken(user);
        return Result<AuthenticatedUser>.Success(
            new AuthenticatedUser(
                user.Id.Value,
                user.Username,
                user.Role,
                issuedToken.Value,
                issuedToken.ExpiresAt));
    }
}
