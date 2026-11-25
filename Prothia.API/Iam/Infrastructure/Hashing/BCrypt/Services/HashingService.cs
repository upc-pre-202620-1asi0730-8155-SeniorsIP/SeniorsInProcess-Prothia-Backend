using Prothia.API.Iam.Application.Internal.OutboundServices;

namespace Prothia.API.Iam.Infrastructure.Hashing.BCrypt.Services;

public sealed class HashingService : IHashingService
{
    private const int WorkFactor = 12;

    public string HashPassword(string password) =>
        global::BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool VerifyPassword(string password, string passwordHash) =>
        global::BCrypt.Net.BCrypt.Verify(password, passwordHash);
}