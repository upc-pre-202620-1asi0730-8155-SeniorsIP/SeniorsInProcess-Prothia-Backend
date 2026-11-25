namespace Prothia.API.Iam.Infrastructure.Tokens.Jwt.Configuration;

public sealed class TokenSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int LifetimeMinutes { get; set; } = 60;
}
