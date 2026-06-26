using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class ProductTempletAttributeConfiguration : IEntityTypeConfiguration<ProductTemplateProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductTemplateProductAttribute> builder)
        {
            builder.HasOne(x => x.ProductTemplate)
                .WithMany(x => x.ProductAttributes)
                .HasForeignKey(x => x.ProductTemplateId);

            builder.HasOne(x => x.ProductAttribute).WithMany(x => x.ProductTemplates)
                .HasForeignKey(x => x.ProductAttributeId);
        }
    }
}
