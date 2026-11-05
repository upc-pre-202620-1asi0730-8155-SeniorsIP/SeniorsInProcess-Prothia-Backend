using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Billing.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyBillingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ClinicSubscription>().HasKey(s => s.Id);
        builder.Entity<ClinicSubscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ClinicSubscription>().Property(s => s.PlanName)
            .HasMaxLength(ClinicSubscription.PlanNameMaxLength).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.Status)
            .HasMaxLength(ClinicSubscription.StatusMaxLength).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.PlanType)
            .HasMaxLength(ClinicSubscription.PlanTypeMaxLength).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.AnnualFee).HasPrecision(18, 2).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.MonthlyEquivalent).HasPrecision(18, 2).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.StartDate)
            .HasMaxLength(ClinicSubscription.DateMaxLength).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.RenewalDate)
            .HasMaxLength(ClinicSubscription.DateMaxLength).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.UsedQuota).IsRequired();
        builder.Entity<ClinicSubscription>().Property(s => s.TotalQuota).IsRequired();
        builder.Entity<ClinicSubscription>().OwnsOne(s => s.PaymentMethod, pm =>
        {
            pm.WithOwner().HasForeignKey("Id");
            pm.Property(p => p.Brand).HasColumnName("payment_method_brand")
                .HasMaxLength(PaymentMethod.BrandMaxLength).IsRequired();
            pm.Property(p => p.CardTitle).HasColumnName("payment_method_card_title")
                .HasMaxLength(PaymentMethod.CardTitleMaxLength).IsRequired();
            pm.Property(p => p.Expiry).HasColumnName("payment_method_expiry")
                .HasMaxLength(PaymentMethod.ExpiryMaxLength).IsRequired();
            pm.Property(p => p.AutoRenew).HasColumnName("payment_method_auto_renew").IsRequired();
        });

        // Electronic invoices
        builder.Entity<ElectronicInvoice>().HasKey(i => i.Id);
        builder.Entity<ElectronicInvoice>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ElectronicInvoice>()
            .Property(i => i.InvoiceNumber)
            .HasConversion(valueObject => valueObject.Value, value => new InvoiceNumber(value))
            .HasMaxLength(InvoiceNumber.MaxLength)
            .IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.IssueDate)
            .HasMaxLength(ElectronicInvoice.IssueDateMaxLength).IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.Concept)
            .HasMaxLength(ElectronicInvoice.ConceptMaxLength).IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.Amount).HasPrecision(18, 2).IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.Currency)
            .HasMaxLength(ElectronicInvoice.CurrencyMaxLength).IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.Status).HasConversion<string>().HasMaxLength(20)
            .IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.StatusLabel)
            .HasMaxLength(ElectronicInvoice.StatusLabelMaxLength).IsRequired();
        builder.Entity<ElectronicInvoice>().Property(i => i.DownloadUrl)
            .HasMaxLength(ElectronicInvoice.DownloadUrlMaxLength).IsRequired();
        builder.Entity<ElectronicInvoice>().HasIndex(i => i.InvoiceNumber).IsUnique(); 
    }
}