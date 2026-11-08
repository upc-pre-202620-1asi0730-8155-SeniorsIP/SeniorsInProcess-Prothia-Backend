using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the persistence mapping of the Communication bounded context.
    /// </summary>
    public static void ApplyCommunicationConfiguration(this ModelBuilder builder)
    {
        var messageIdConverter = new StronglyTypedIdConverter<ClinicalMessageId, int>(id => id.Value, v => new ClinicalMessageId(v));
        var transmissionIdConverter = new StronglyTypedIdConverter<ClinicalTransmissionId, int>(id => id.Value, v => new ClinicalTransmissionId(v));
        var patientIdConverter = new StronglyTypedIdConverter<PatientId, int>(id => id.Value, v => new PatientId(v));

        // Clinical messages
        builder.Entity<ClinicalMessage>().HasKey(m => m.Id);
        builder.Entity<ClinicalMessage>()
            .Property(m => m.Id)
            .HasConversion(messageIdConverter)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ClinicalMessage>().Property(m => m.Sender).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<ClinicalMessage>()
            .Property(m => m.SenderName)
            .HasMaxLength(ClinicalMessage.SenderNameMaxLength)
            .IsRequired();
        builder.Entity<ClinicalMessage>()
            .Property(m => m.SenderRole)
            .HasMaxLength(ClinicalMessage.SenderRoleMaxLength)
            .IsRequired();
        builder.Entity<ClinicalMessage>()
            .Property(m => m.Text)
            .HasConversion(valueObject => valueObject.Value, value => new MessageText(value))
            .HasMaxLength(MessageText.MaxLength)
            .IsRequired();
        builder.Entity<ClinicalMessage>()
            .Property(m => m.Timestamp)
            .HasMaxLength(ClinicalMessage.TimestampMaxLength)
            .IsRequired();
        builder.Entity<ClinicalMessage>().Property(m => m.IsSystem).IsRequired();
        builder.Entity<ClinicalMessage>().Property(m => m.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

        // Clinical transmissions
        var sharedModulesComparer = new ValueComparer<IReadOnlyList<string>>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Entity<ClinicalTransmission>().HasKey(t => t.Id);
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.Id)
            .HasConversion(transmissionIdConverter)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.PatientId)
            .HasConversion(patientIdConverter)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.PatientName)
            .HasMaxLength(ClinicalTransmission.PatientNameMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.PatientDni)
            .HasMaxLength(ClinicalTransmission.PatientDniMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.WorkshopName)
            .HasConversion(valueObject => valueObject.Value, value => new WorkshopName(value))
            .HasMaxLength(WorkshopName.MaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.SharedModules)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList(),
                sharedModulesComparer)
            .HasMaxLength(500)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.TechnicalNotes)
            .HasMaxLength(ClinicalTransmission.TechnicalNotesMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.StatusLabel)
            .HasMaxLength(ClinicalTransmission.StatusLabelMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.Summary)
            .HasMaxLength(ClinicalTransmission.SummaryMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>()
            .Property(t => t.CreatedAtDate)
            .HasMaxLength(ClinicalTransmission.CreatedAtDateMaxLength)
            .IsRequired();
        builder.Entity<ClinicalTransmission>().HasIndex(t => t.PatientId);
    }
}
