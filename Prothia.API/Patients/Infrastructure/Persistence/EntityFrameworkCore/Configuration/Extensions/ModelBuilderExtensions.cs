using Microsoft.EntityFrameworkCore;
using Prothia.API.Patients.Domain.Model.Aggregates;
using Prothia.API.Patients.Domain.Model.ValueObjects;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

namespace Prothia.API.Patients.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyPatientsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Patient>().HasKey(p => p.Id);
        builder.Entity<Patient>().Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Patient>()
            .Property(p => p.FullName)
            .HasConversion(valueObject => valueObject.Value, value => new FullName(value))
            .HasMaxLength(FullName.MaxLength)
            .IsRequired();
        builder.Entity<Patient>()
            .Property(p => p.Dni)
            .HasConversion(valueObject => valueObject.Value, value => new Dni(value))
            .HasMaxLength(Dni.Length)
            .IsRequired();
        builder.Entity<Patient>().Property(p => p.Age).IsRequired();
        builder.Entity<Patient>()
            .Property(p => p.EAmputation)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
        builder.Entity<Patient>()
            .Property(p => p.EkLevel)
            .HasConversion<string>()
            .HasMaxLength(2)
            .IsRequired();
        builder.Entity<Patient>()
            .Property(p => p.ProsthesisCode)
            .HasConversion(valueObject => valueObject!.Value, value => new ProsthesisCode(value))
            .HasMaxLength(ProsthesisCode.MaxLength);
        builder.Entity<Patient>().Property(p => p.Adherence).IsRequired();
        builder.Entity<Patient>().Property(p => p.Symmetry).IsRequired();
        builder.Entity<Patient>()
            .Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Entity<Patient>().HasIndex(p => p.Dni).IsUnique();
    }
}