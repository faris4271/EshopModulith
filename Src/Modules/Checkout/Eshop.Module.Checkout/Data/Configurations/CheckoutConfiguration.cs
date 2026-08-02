using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Checkout.Data.Configurations;

public sealed class CheckoutConfiguration : IEntityTypeConfiguration<Entities.Checkout>
{
    public void Configure(EntityTypeBuilder<Entities.Checkout> builder)
    {
        builder.ToTable("checkouts", "checkout");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.OwnsOne(x => x.ShippingAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.ComplexProperty(x => x.TaxAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });
        builder.ComplexProperty(x => x.ShippingAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.ComplexProperty(x => x.TaxAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });
        builder.ComplexProperty(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount).HasPrecision(18, 2);
            money.Property(m => m.Currency).HasMaxLength(3);
        });

        builder.Property(x => x.CouponCode).HasMaxLength(450);
        builder.Property(x => x.CouponRuleName).HasMaxLength(450);
        builder.Property(x => x.ShippingMethod).HasMaxLength(450);
        builder.Property(x => x.OrderNote).HasMaxLength(1000);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

    }
}
