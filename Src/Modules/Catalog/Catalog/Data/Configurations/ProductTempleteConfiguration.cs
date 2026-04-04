using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal sealed class ProductTempleteConfiguration : IEntityTypeConfiguration<ProductTemplate>
    {
        public void Configure(EntityTypeBuilder<ProductTemplate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(c => c.Name).HasConversion(a => a.name, name => new(name));

        }
    }
}
