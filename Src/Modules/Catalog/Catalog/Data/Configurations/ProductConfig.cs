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

            builder.HasOne(x => x.Brand).WithMany(x=>x.Products).HasForeignKey(x => x.BrandId);

        }
    }
}
