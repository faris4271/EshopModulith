using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Categories).HasForeignKey(x => x.ProductId);

        }
    }
}
