using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Application.Model;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Infrastructure.Tokens.Jwt.Configuration;

namespace Prothia.API.Iam.Infrastructure.Tokens.Jwt.Services;

public sealed class TokenService(IOptions<TokenSettings> options) : ITokenService
{
    private readonly TokenSettings _settings = options.Value;

    public IssuedToken GenerateToken(User user)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(_settings.LifetimeMinutes);
        var userId = user.Id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _settings.Issuer,
            _settings.Audience,
            claims,
            now.UtcDateTime,
            expiresAt.UtcDateTime,
            credentials);

        return new IssuedToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
