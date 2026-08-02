using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Pricing.Data.Configurations;

internal sealed class CartRuleConfiguration : IEntityTypeConfiguration<CartRule>
{
    public void Configure(EntityTypeBuilder<CartRule> builder)
    {
        builder.ToTable("CartRules", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Description);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.StartOn);

        builder.Property(x => x.EndOn);

        builder.Property(x => x.IsCouponRequired)
            .IsRequired();

        builder.Property(x => x.RuleToApply)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.MaxDiscountAmount)
            .HasPrecision(18, 4);

        builder.Property(x => x.DiscountStep);

        builder.Property(x => x.UsageLimitPerCoupon);

        builder.Property(x => x.UsageLimitPerCustomer);

        builder.HasMany(x => x.Coupons)
            .WithOne(x => x.CartRule)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.CustomerGroups)
            .WithOne(x => x.CartRule)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Products)
            .WithOne(x => x.CartRule)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Categories)
            .WithOne(x => x.CartRule)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
