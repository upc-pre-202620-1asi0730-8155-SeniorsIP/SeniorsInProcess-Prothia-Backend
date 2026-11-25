using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.Commands;
using Prothia.API.Shared.Application.Model;

namespace Prothia.API.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);
    Task<Result<AuthenticatedUser>> Handle(SignInCommand command, CancellationToken cancellationToken = default);
}