using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configurations
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category.Models.Category>
    {
        public void Configure(EntityTypeBuilder<Category.Models.Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(category => category.Name).IsRequired();
            builder.HasOne(c => c.Parent).
                WithMany(c => c.Children).HasForeignKey(c => c.ParentId);

            builder.Property(c => c.Name).HasConversion(a => a.name, name => new(name));
            builder.Property(d => d.Description)
                .HasConversion(d => d.description, v => new Shared.DDD.Description(v));

        }
    }
}
