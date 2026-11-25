using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Domain.Model.Aggregates;

namespace Prothia.API.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    IssuedToken GenerateToken(User user);
}