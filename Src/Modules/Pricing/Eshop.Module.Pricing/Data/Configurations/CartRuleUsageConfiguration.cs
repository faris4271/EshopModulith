using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Pricing.Data.Configurations;

internal sealed class CartRuleUsageConfiguration : IEntityTypeConfiguration<CartRuleUsage>
{
    public void Configure(EntityTypeBuilder<CartRuleUsage> builder)
    {
        builder.ToTable("CartRuleUsages", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CartRuleId)
            .IsRequired();

        builder.Property(x => x.CouponId);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.HasOne(x => x.CartRule)
            .WithMany()
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Coupon)
            .WithMany()
            .HasForeignKey(x => x.CouponId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
