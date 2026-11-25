using Microsoft.EntityFrameworkCore;
using Prothia.API.Iam.Domain.Model.Aggregates;
using Prothia.API.Iam.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Id)
                .HasConversion(new StronglyTypedIdConverter<UserId, int>(id => id.Value, value => new UserId(value)))
                .ValueGeneratedOnAdd()
                .IsRequired();
            entity.Property(user => user.Username).HasMaxLength(User.UsernameMaxLength).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(User.PasswordHashMaxLength).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(User.RoleMaxLength).IsRequired();
            entity.Property(user => user.CreatedAt);
            entity.Property(user => user.UpdatedAt);
            entity.HasIndex(user => user.Username).IsUnique();
        });
    }

}