using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ProductTierPriceConfiguration : IEntityTypeConfiguration<ProductTierPrice>
{
    public void Configure(EntityTypeBuilder<ProductTierPrice> builder)
    {
        builder.ToTable("ProductTierPrices", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductPriceId)
            .IsRequired();

        builder.Property(x => x.MinQuantity)
            .IsRequired();

        builder.Property(x => x.DiscountedPrice)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.HasIndex(x => new { x.ProductPriceId, x.MinQuantity })
            .IsUnique();
    }
}
