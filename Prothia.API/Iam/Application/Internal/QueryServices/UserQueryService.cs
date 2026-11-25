using Prothia.API.Iam.Application.QueryServices;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.Queries;
using Prothia.API.Iam.Domain.Repositories;

namespace Prothia.API.Iam.Application.Internal.QueryServices;

public sealed class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public Task<IEnumerable<User>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken = default) =>
        userRepository.ListAsNoTrackingAsync(cancellationToken);

    public Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default) =>
        userRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);

    public Task<User?> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken = default) =>
        userRepository.FindByUsernameAsync(query.Username.Trim().ToLowerInvariant(), cancellationToken);
}
