using Microsoft.EntityFrameworkCore;
using Prothia.API.Analytics.Domain.Model.Aggregates;
using Prothia.API.Analytics.Domain.Model.Entities;
using Prothia.API.Analytics.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        // Clinical reports
        builder.Entity<ClinicalReport>().HasKey(r => r.Id);
        builder.Entity<ClinicalReport>().Property(r => r.Id)
            .HasConversion(new StronglyTypedIdConverter<ClinicalReportId, int>(id => id.Value, value => new ClinicalReportId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ClinicalReport>()
            .Property(r => r.ReportCode)
            .HasConversion(valueObject => valueObject.Value, value => new ReportCode(value))
            .HasMaxLength(ReportCode.MaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.IssueDate).HasMaxLength(ClinicalReport.IssueDateMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.ClinicName).HasMaxLength(ClinicalReport.ClinicNameMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.ClinicUnit).HasMaxLength(ClinicalReport.ClinicUnitMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>()
            .Property(r => r.PatientId)
            .HasConversion(new StronglyTypedIdConverter<PatientId, int>(id => id.Value, value => new PatientId(value)))
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.PatientName).HasMaxLength(ClinicalReport.PatientNameMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.Dni).HasMaxLength(ClinicalReport.DniMaxLength).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.Age).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.Diagnosis).HasMaxLength(ClinicalReport.DiagnosisMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.ProsthesisCode)
            .HasMaxLength(ClinicalReport.ProsthesisCodeMaxLength).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.EvaluationPeriod)
            .HasMaxLength(ClinicalReport.EvaluationPeriodMaxLength).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.Summary).HasMaxLength(ClinicalReport.SummaryMaxLength)
            .IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.MedicalRecommendation)
            .HasMaxLength(ClinicalReport.MedicalRecommendationMaxLength).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.TherapistName)
            .HasMaxLength(ClinicalReport.TherapistNameMaxLength).IsRequired();
        builder.Entity<ClinicalReport>().Property(r => r.TherapistRole)
            .HasMaxLength(ClinicalReport.TherapistRoleMaxLength).IsRequired();
        builder.Entity<ClinicalReport>()
            .Property(r => r.TherapistRegistry)
            .HasConversion(valueObject => valueObject.Value, value => new TherapistRegistry(value))
            .HasMaxLength(TherapistRegistry.MaxLength)
            .IsRequired();

        builder.Entity<ClinicalReport>()
            .HasMany(r => r.Indicators)
            .WithOne()
            .HasForeignKey(i => i.ClinicalReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ClinicalReport>().HasIndex(r => r.ReportCode).IsUnique();
        builder.Entity<ClinicalReport>().HasIndex(r => r.PatientId);

        // Report indicators
        builder.Entity<ReportIndicator>().HasKey(i => i.Id);
        builder.Entity<ReportIndicator>().Property(i => i.Id)
            .HasConversion(new StronglyTypedIdConverter<ReportIndicatorId, int>(id => id.Value, value => new ReportIndicatorId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ReportIndicator>().Property(i => i.ClinicalReportId)
            .HasConversion(new StronglyTypedIdConverter<ClinicalReportId, int>(id => id.Value, value => new ClinicalReportId(value)))
            .IsRequired();
        builder.Entity<ReportIndicator>().Property(i => i.Name).HasMaxLength(ReportIndicator.NameMaxLength)
            .IsRequired();
        builder.Entity<ReportIndicator>().Property(i => i.Value).HasMaxLength(ReportIndicator.ValueMaxLength)
            .IsRequired();
        builder.Entity<ReportIndicator>().Property(i => i.NormalRange)
            .HasMaxLength(ReportIndicator.NormalRangeMaxLength).IsRequired();
        builder.Entity<ReportIndicator>().Property(i => i.Evaluation)
            .HasMaxLength(ReportIndicator.EvaluationMaxLength).IsRequired();
        builder.Entity<ReportIndicator>().Property(i => i.Status).HasConversion<string>().HasMaxLength(20)
            .IsRequired();
    }
}
