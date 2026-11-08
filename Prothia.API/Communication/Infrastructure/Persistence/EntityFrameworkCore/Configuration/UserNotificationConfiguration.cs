using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prothia.API.Communication.Domain.Model.Aggregates;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Property(n => n.UserId).IsRequired();
        
        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<string>() 
            .HasMaxLength(50);
            
        builder.Property(n => n.ReferenceId).IsRequired(false);
        builder.Property(n => n.ReadAt).IsRequired(false);
    }
}