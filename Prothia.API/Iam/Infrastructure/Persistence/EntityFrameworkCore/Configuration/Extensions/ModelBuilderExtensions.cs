using Microsoft.EntityFrameworkCore;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        var user = builder.Entity<User>();

        user.HasKey(u => u.Id);
        user.Property(u => u.Id)
            .HasConversion(new StronglyTypedIdConverter<UserId, int>(id => id.Value, value => new UserId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();

        user.Property(u => u.Username)
            .HasMaxLength(64)
            .IsRequired();

        user.Property(u => u.NormalizedUsername)
            .HasMaxLength(64)
            .IsRequired();
        user.HasIndex(u => u.NormalizedUsername).IsUnique();

        user.Property(u => u.FullName)
            .HasMaxLength(120)
            .IsRequired();

        user.Property(u => u.Email)
            .HasMaxLength(160)
            .IsRequired();
        user.HasIndex(u => u.Email).IsUnique();

        user.Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        user.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        user.Property(u => u.PatientId)
            .IsRequired(false);
        user.HasIndex(u => u.PatientId);

        user.Property(u => u.CreatedByUserId)
            .IsRequired(false);
        user.HasIndex(u => u.CreatedByUserId);

        user.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        user.Ignore(u => u.DomainEvents);
    }
}
