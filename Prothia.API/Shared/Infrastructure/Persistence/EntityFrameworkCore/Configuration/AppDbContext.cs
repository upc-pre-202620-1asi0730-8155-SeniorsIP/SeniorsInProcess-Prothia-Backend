using Microsoft.EntityFrameworkCore;
using Prothia.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Billing.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Patients.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Prescriptions.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Each bounded context owns the mapping of its aggregates.
        builder.ApplyPatientsConfiguration();
        builder.ApplyAnalyticsConfiguration();
        builder.ApplyBillingConfiguration();
        builder.ApplyCommunicationConfiguration();
        builder.ApplyMonitoringConfiguration();
        builder.ApplyPrescriptionsConfiguration();
        builder.ApplyWorkshopConfiguration();
        builder.UseSnakeCaseNamingConvention();
    }
}
