using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Iam.Infrastructure.Tokens.Jwt.Configuration;

namespace Prothia.API.Iam.Infrastructure.Tokens.Jwt.Services;

public class TokenService(
    IOptions<TokenSettings> tokenSettings,
    ILogger<TokenService> logger) : ITokenService
{
    private readonly TokenSettings _tokenSettings = tokenSettings.Value;

    public string GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var keyBytes = _tokenSettings.GetValidatedSecretBytes();
        var signingKey = new SymmetricSecurityKey(keyBytes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("fullName", user.FullName)
        };

        if (user.PatientId.HasValue)
            claims.Add(new Claim("patientId", user.PatientId.Value.ToString()));

        if (user.CreatedByUserId.HasValue)
            claims.Add(new Claim("createdByUserId", user.CreatedByUserId.Value.ToString()));

        var now = DateTime.UtcNow;
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(_tokenSettings.ExpirationMinutes),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }

    public async Task<UserId?> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        try
        {
            var keyBytes = _tokenSettings.GetValidatedSecretBytes();
            var handler = new JsonWebTokenHandler();

            var result = await handler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _tokenSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            });

            if (!result.IsValid || result.ClaimsIdentity is null)
            {
                logger.LogWarning("JWT validation failed: {Reason}", result.Exception?.Message ?? "Invalid token.");
                return null;
            }

            var subClaim = result.ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return int.TryParse(subClaim, out var userId) && userId > 0
                ? new UserId(userId)
                : null;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Exception occurred while validating JWT.");
            return null;
        }
    }
}
