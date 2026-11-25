using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.Queries;

namespace Prothia.API.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default);
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);
    Task<User?> Handle(GetUserByUsernameQuery query, CancellationToken cancellationToken = default);
}