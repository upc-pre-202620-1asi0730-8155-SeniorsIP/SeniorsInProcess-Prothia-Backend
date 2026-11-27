using System.Text;

namespace Prothia.API.Iam.Infrastructure.Tokens.Jwt.Configuration;

public class TokenSettings
{
    public const string SectionName = "TokenSettings";
    public const int MinimumSecretByteLength = 32;

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Prothia.API";
    public string Audience { get; set; } = "Prothia.Client";
    public int ExpirationMinutes { get; set; } = 480;

    public byte[] GetValidatedSecretBytes()
    {
        if (string.IsNullOrWhiteSpace(Secret))
            throw new InvalidOperationException("TokenSettings:Secret must be configured.");

        var keyBytes = Encoding.UTF8.GetBytes(Secret);
        if (keyBytes.Length < MinimumSecretByteLength)
            throw new InvalidOperationException(
                $"TokenSettings:Secret must be at least {MinimumSecretByteLength} UTF-8 bytes (256 bits) for HS256.");

        if (string.IsNullOrWhiteSpace(Issuer))
            throw new InvalidOperationException("TokenSettings:Issuer must be configured.");

        if (string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException("TokenSettings:Audience must be configured.");

        if (ExpirationMinutes <= 0)
            throw new InvalidOperationException("TokenSettings:ExpirationMinutes must be greater than zero.");

        return keyBytes;
    }
}
