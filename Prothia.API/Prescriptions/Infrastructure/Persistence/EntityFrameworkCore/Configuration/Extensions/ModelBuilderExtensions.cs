using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prothia.API.Prescriptions.Domain.Model.Aggregates;
using Prothia.API.Prescriptions.Domain.Model.Entities;
using Prothia.API.Prescriptions.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Prescriptions.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyPrescriptionsConfiguration(this ModelBuilder builder)
    {
        var focusComparer = new ValueComparer<IReadOnlyList<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        // Exercises
        builder.Entity<Exercise>().HasKey(e => e.Id);
        builder.Entity<Exercise>().Property(e => e.Id)
            .HasConversion(new StronglyTypedIdConverter<ExerciseId, int>(id => id.Value, value => new ExerciseId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Exercise>()
            .Property(e => e.Name)
            .HasConversion(valueObject => valueObject.Value, value => new ExerciseName(value))
            .HasMaxLength(ExerciseName.MaxLength)
            .IsRequired();
        builder.Entity<Exercise>().HasIndex(e => e.Name).IsUnique();
        builder.Entity<Exercise>()
            .Property(e => e.Focus)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList(),
                focusComparer)
            .HasMaxLength(Exercise.FocusMaxLength)
            .IsRequired();

        // Prescriptions
        builder.Entity<Prescription>().HasKey(p => p.Id);
        builder.Entity<Prescription>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<PrescriptionId, int>(id => id.Value,
                value => new PrescriptionId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Prescription>()
            .Property(p => p.PatientId)
            .HasConversion(new StronglyTypedIdConverter<PatientId, int>(id => id.Value, value => new PatientId(value)))
            .IsRequired();
        builder.Entity<Prescription>().Property(p => p.PhaseNumber).IsRequired();
        builder.Entity<Prescription>()
            .Property(p => p.PhaseName)
            .HasConversion(valueObject => valueObject.Value, value => new PhaseName(value))
            .HasMaxLength(PhaseName.MaxLength)
            .IsRequired();
        builder.Entity<Prescription>().Property(p => p.DurationWeeks).IsRequired();
        builder.Entity<Prescription>().Property(p => p.FrequencyPerWeek).IsRequired();
        builder.Entity<Prescription>()
            .HasMany(p => p.Exercises)
            .WithOne()
            .HasForeignKey(e => e.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Prescription>().HasIndex(p => p.PatientId);

        // PrescribedExercises
        builder.Entity<PrescribedExercise>().HasKey(e => e.Id);
        builder.Entity<PrescribedExercise>().Property(e => e.Id)
            .HasConversion(new StronglyTypedIdConverter<PrescribedExerciseId, int>(id => id.Value,
                value => new PrescribedExerciseId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<PrescribedExercise>().Property(e => e.PrescriptionId)
            .HasConversion(new StronglyTypedIdConverter<PrescriptionId, int>(id => id.Value,
                value => new PrescriptionId(value)))
            .IsRequired();
        builder.Entity<PrescribedExercise>().Property(e => e.ExerciseId)
            .HasConversion(new StronglyTypedIdConverter<ExerciseId, int>(id => id.Value,
                value => new ExerciseId(value)))
            .IsRequired();
        builder.Entity<PrescribedExercise>().Property(e => e.Name).HasMaxLength(PrescribedExercise.NameMaxLength).IsRequired();
        builder.Entity<PrescribedExercise>().Property(e => e.Sets);
        builder.Entity<PrescribedExercise>().Property(e => e.Reps);
        builder.Entity<PrescribedExercise>().Property(e => e.RestSeconds);
    }
}
