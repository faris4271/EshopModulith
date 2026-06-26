using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal sealed class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever();

            builder.Property(x => x.AttributeId)
                   .HasColumnName("AttributeId")
                   .ValueGeneratedNever()
                   .IsRequired();

            builder.Property(x => x.AttributeId)
                .ValueGeneratedNever()
                .IsRequired();


            builder.HasOne(x => x.Attribute)
                .WithMany(x => x.AttributeValues)
                .HasForeignKey(x => x.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
