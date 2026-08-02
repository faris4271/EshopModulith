using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.ToTable("ProductPrices", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.BasePrice)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.BaseCurrency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.EffectiveFromUtc)
            .IsRequired();

        builder.Property(x => x.EffectiveToUtc);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasMany(x => x.TierPrices)
            .WithOne()
            .HasForeignKey(x => x.ProductPriceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ProductId, x.EffectiveFromUtc, x.IsActive });
    }
}
