using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class ProductLinkConfiguration : IEntityTypeConfiguration<ProductLink>
    {
        public void Configure(EntityTypeBuilder<ProductLink> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Product)
             .WithMany(x => x.ProductLinks)
              .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LinkedProduct)
                  .WithMany(x => x.LinkedProductLinks)
                  .HasForeignKey(x => x.LinkedProductId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
