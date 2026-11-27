using Microsoft.EntityFrameworkCore;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Iam.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeUsername(username);
        return await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.NormalizedUsername == normalized, cancellationToken);
    }

    public async Task<User?> FindByUsernameAsNoTrackingAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeUsername(username);
        return await Context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUsername == normalized, cancellationToken);
    }

    public async Task<User?> FindByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> FindByIdAsNoTrackingAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public override Task<User?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
    {
        return FindByIdAsNoTrackingAsync(new UserId(id), cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeUsername(username);
        return await Context.Set<User>()
            .AnyAsync(u => u.NormalizedUsername == normalized, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = NormalizeEmail(email);
        return await Context.Set<User>()
            .AnyAsync(u => u.Email == normalized, cancellationToken);
    }

    public async Task<IEnumerable<User>> ListByRoleAsNoTrackingAsync(ERole? role, int? clinicOwnerUserId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<User>().AsNoTracking();
        if (clinicOwnerUserId.HasValue)
            query = query.Where(u => u.CreatedByUserId == clinicOwnerUserId.Value || u.Id == new UserId(clinicOwnerUserId.Value));

        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        return await query
            .OrderBy(u => u.Id)
            .ToListAsync(cancellationToken);
    }

    private static string NormalizeUsername(string username) =>
        (username ?? string.Empty).Trim().ToLowerInvariant();

    private static string NormalizeEmail(string email) =>
        (email ?? string.Empty).Trim().ToLowerInvariant();
}
