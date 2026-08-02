using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasConversion(a => a.name, name => new(name));
            builder.Property(d => d.Description)
                .HasConversion(d => d.description, v => new Shared.DDD.Description(v));
            builder.Property(d => d.ShortDescription)
               .HasConversion(d => d.description, v => new Shared.DDD.Description(v));

            builder.HasOne(x => x.Brand).WithMany(x => x.Products).HasForeignKey(x => x.BrandId);

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

            builder.HasMany(x => x.OptionCombinations).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);



        }
    }
}
