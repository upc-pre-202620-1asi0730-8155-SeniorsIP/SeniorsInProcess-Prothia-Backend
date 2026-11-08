using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prothia.API.Communication.Domain.Model.Aggregates;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.Property(m => m.SenderUserId).IsRequired();
        builder.Property(m => m.RecipientUserId).IsRequired();
        
        builder.Property(m => m.Body)
            .IsRequired()
            .HasMaxLength(1000); // Límite razonable para un mensaje

        builder.Property(m => m.SentAt).IsRequired();
    }
}