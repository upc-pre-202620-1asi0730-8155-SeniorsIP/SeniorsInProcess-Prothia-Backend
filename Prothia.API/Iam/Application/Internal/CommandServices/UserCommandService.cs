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
            password is null ||
            password.Length < MinimumPasswordLength ||
            Encoding.UTF8.GetByteCount(password) > MaximumPasswordBytes)
        {
            return Result<User>.Failure(IamErrors.InvalidRegistrationData, IamError.InvalidRegistrationData);
        }

        if (await userRepository.ExistsByUsernameAsync(username, cancellationToken))
            return Result<User>.Failure(IamErrors.UsernameAlreadyTaken, IamError.UsernameAlreadyTaken);

        var user = new User(username, hashingService.HashPassword(password));
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
