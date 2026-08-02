using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class ProductPriceHistoryConfiguration : IEntityTypeConfiguration<ProductPriceHistory>
    {
        public void Configure(EntityTypeBuilder<ProductPriceHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ComplexProperty(x => x.Price, price =>
            {
                price.Property(p => p.Amount).HasColumnName("Price_Amount");
                price.Property(p => p.Currency).HasColumnName("Price_Currency");
            });

            builder.ComplexProperty(x => x.OldPrice, oldPrice =>
            {
                oldPrice.Property(p => p.Amount).HasColumnName("OldPrice_Amount");
                oldPrice.Property(p => p.Currency).HasColumnName("OldPrice_Currency");
            });

            builder.ComplexProperty(x => x.SpecialPrice, specialPrice =>
            {
                specialPrice.Property(p => p.Amount).HasColumnName("SpecialPrice_Amount");
                specialPrice.Property(p => p.Currency).HasColumnName("SpecialPrice_Currency");
            });
        }
    }
}
