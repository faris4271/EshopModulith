using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Pricing.Data.Configurations;

internal sealed class CartRuleProductConfiguration : IEntityTypeConfiguration<CartRuleProduct>
{
    public void Configure(EntityTypeBuilder<CartRuleProduct> builder)
    {
        builder.ToTable("CartRuleProducts", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.CartRuleId)
            .IsRequired();

        builder.HasOne(x => x.CartRule)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CartRuleId, x.ProductId })
            .IsUnique();
    }
}
