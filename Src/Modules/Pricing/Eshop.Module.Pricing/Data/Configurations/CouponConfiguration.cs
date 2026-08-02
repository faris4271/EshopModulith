using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Pricing.Data.Configurations;

internal sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.HasOne(x => x.CartRule)
            .WithMany(x => x.Coupons)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CartRuleId, x.Code })
            .IsUnique();
    }
}
