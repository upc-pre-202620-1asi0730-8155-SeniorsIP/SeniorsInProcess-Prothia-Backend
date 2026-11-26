using Microsoft.EntityFrameworkCore;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Iam.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class UserRepository(AppDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    public Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        Context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username == username, cancellationToken);

    public Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        Context.Set<User>().AnyAsync(user => user.Username == username, cancellationToken);

    public Task<bool> ExistsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default) =>
        Context.Set<User>().AnyAsync(user => user.PatientId == patientId, cancellationToken);

    public override Task<User?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default) =>
        Context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == new UserId(id), cancellationToken);
}