using Microsoft.EntityFrameworkCore;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Prothia.API.Workshop.Domain.Model.Aggregates;
using Prothia.API.Workshop.Domain.Model.ValueObjects;

namespace Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyWorkshopConfiguration(this ModelBuilder builder)
    {
        // Prostheses
        builder.Entity<Prosthesis>().HasKey(p => p.Id);
        builder.Entity<Prosthesis>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<ProsthesisId, int>(id => id.Value, value => new ProsthesisId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Prosthesis>()
            .Property(p => p.SerialNumber)
            .HasConversion(valueObject => valueObject.Value, value => new SerialNumber(value))
            .HasMaxLength(SerialNumber.MaxLength)
            .IsRequired();
        builder.Entity<Prosthesis>().HasIndex(p => p.SerialNumber).IsUnique();
        builder.Entity<Prosthesis>().Property(p => p.Type).HasMaxLength(Prosthesis.TypeMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.PatientName).HasMaxLength(Prosthesis.PatientNameMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.ClinicName).HasMaxLength(Prosthesis.ClinicNameMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.AccumulatedCycles).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.CycleLimit).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.LastService).HasMaxLength(Prosthesis.LastServiceMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.BatteryLevel).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.SensorStatus).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.KLevel).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.SocketType).HasMaxLength(Prosthesis.ComponentTypeMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.KneeType).HasMaxLength(Prosthesis.ComponentTypeMaxLength).IsRequired();
        builder.Entity<Prosthesis>().Property(p => p.FootType).HasMaxLength(Prosthesis.ComponentTypeMaxLength).IsRequired();

        // MechanicalAlerts
        builder.Entity<MechanicalAlert>().HasKey(a => a.Id);
        builder.Entity<MechanicalAlert>().Property(a => a.Id)
            .HasConversion(new StronglyTypedIdConverter<MechanicalAlertId, int>(id => id.Value, value => new MechanicalAlertId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<MechanicalAlert>().Property(a => a.ProsthesisId).IsRequired();
        builder.Entity<MechanicalAlert>()
            .Property(a => a.SerialNumber)
            .HasConversion(valueObject => valueObject.Value, value => new SerialNumber(value))
            .HasMaxLength(SerialNumber.MaxLength)
            .IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.Cycles).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.CycleLimit).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.PatientName).HasMaxLength(MechanicalAlert.PatientNameMaxLength).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.KLevel).HasConversion<string>().HasMaxLength(10).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.Component).HasMaxLength(MechanicalAlert.ComponentMaxLength).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.Description).HasMaxLength(MechanicalAlert.DescriptionMaxLength).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.Severity).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<MechanicalAlert>().Property(a => a.OccurredAt).HasMaxLength(MechanicalAlert.OccurredAtMaxLength).IsRequired();
        builder.Entity<MechanicalAlert>().HasIndex(a => new { a.ProsthesisId, a.Status });

        // MaintenanceSchedules
        builder.Entity<MaintenanceSchedule>().HasKey(m => m.Id);
        builder.Entity<MaintenanceSchedule>().Property(m => m.Id)
            .HasConversion(new StronglyTypedIdConverter<MaintenanceScheduleId, int>(id => id.Value, value => new MaintenanceScheduleId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<MaintenanceSchedule>()
            .Property(m => m.SerialNumber)
            .HasConversion(valueObject => valueObject.Value, value => new SerialNumber(value))
            .HasMaxLength(SerialNumber.MaxLength)
            .IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.PatientName).HasMaxLength(MaintenanceSchedule.PatientNameMaxLength).IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.ScheduledDate).HasMaxLength(MaintenanceSchedule.ScheduledDateMaxLength).IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.InterventionType).HasMaxLength(MaintenanceSchedule.InterventionTypeMaxLength).IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.TechnicianName).HasMaxLength(MaintenanceSchedule.TechnicianNameMaxLength).IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<MaintenanceSchedule>().Property(m => m.Notes).HasMaxLength(MaintenanceSchedule.NotesMaxLength).IsRequired();
    }
}
