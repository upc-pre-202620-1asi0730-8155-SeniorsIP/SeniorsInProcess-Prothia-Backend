using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClinicSubscriptionRepository(AppDbContext context)
    : BaseRepository<ClinicSubscription>(context), IClinicSubscriptionRepository;