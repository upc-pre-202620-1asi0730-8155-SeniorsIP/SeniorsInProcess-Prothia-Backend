using Microsoft.EntityFrameworkCore;
using Prothia.API.Monitoring.Domain.Model.Aggregates;
using Prothia.API.Monitoring.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{

    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id)
            .HasConversion(new StronglyTypedIdConverter<AlertId, int>(id => id.Value, value => new AlertId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Alert>()
            .Property(a => a.PatientId)
            .HasConversion(valueObject => valueObject.Value, value => new PatientId(value))
            .IsRequired();
        builder.Entity<Alert>().Property(a => a.PatientName).HasMaxLength(Alert.PatientNameMaxLength).IsRequired();
        builder.Entity<Alert>()
            .Property(a => a.DeviationType)
            .HasConversion(valueObject => valueObject.Value, value => new DeviationType(value))
            .HasMaxLength(DeviationType.MaxLength)
            .IsRequired();
        builder.Entity<Alert>()
            .Property(a => a.RecordedValue)
            .HasConversion(valueObject => valueObject.Value, value => new ReadingText(value))
            .HasMaxLength(ReadingText.MaxLength)
            .IsRequired();
        builder.Entity<Alert>()
            .Property(a => a.Threshold)
            .HasConversion(valueObject => valueObject.Value, value => new ReadingText(value))
            .HasMaxLength(ReadingText.MaxLength)
            .IsRequired();
        builder.Entity<Alert>().Property(a => a.Severity).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Alert>().Property(a => a.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Alert>().Property(a => a.OccurredAt).IsRequired();
        builder.Entity<Alert>().Property(a => a.ResolvedAt);
        builder.Entity<Alert>().HasIndex(a => new { a.PatientId, a.Status });
        
        builder.Entity<ThresholdProfile>().HasKey(t => t.Id);
        builder.Entity<ThresholdProfile>().Property(t => t.Id)
            .HasConversion(new StronglyTypedIdConverter<ThresholdProfileId, int>(id => id.Value, value => new ThresholdProfileId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ThresholdProfile>()
            .Property(t => t.PatientId)
            .HasConversion(valueObject => valueObject.Value, value => new PatientId(value))
            .IsRequired();
        builder.Entity<ThresholdProfile>().Property(t => t.PelvicTilt).IsRequired();
        builder.Entity<ThresholdProfile>().Property(t => t.Asymmetry).IsRequired();
        builder.Entity<ThresholdProfile>().Property(t => t.Cadence).IsRequired();
        builder.Entity<ThresholdProfile>().Property(t => t.ImpactPeak).IsRequired();
        builder.Entity<ThresholdProfile>().HasIndex(t => t.PatientId).IsUnique();
        
        builder.Entity<GaitSession>().HasKey(s => s.Id);
        builder.Entity<GaitSession>().Property(s => s.Id)
            .HasConversion(new StronglyTypedIdConverter<GaitSessionId, int>(id => id.Value, value => new GaitSessionId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<GaitSession>()
            .Property(s => s.PatientId)
            .HasConversion(valueObject => valueObject.Value, value => new PatientId(value))
            .IsRequired();
        builder.Entity<GaitSession>().Property(s => s.RecordedAt).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.DurationMinutes).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.Steps).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.Cadence).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.SymmetryPercent).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.AlertsCount).IsRequired();
        builder.Entity<GaitSession>().Property(s => s.ImpactPeakNewtons).IsRequired();
        builder.Entity<GaitSession>().HasIndex(s => new { s.PatientId, s.RecordedAt });
    }
}
