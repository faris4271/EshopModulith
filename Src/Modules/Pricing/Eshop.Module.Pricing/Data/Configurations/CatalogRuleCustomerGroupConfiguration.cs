using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CatalogRuleCustomerGroupConfiguration : IEntityTypeConfiguration<CatalogRuleCustomerGroup>
{
    public void Configure(EntityTypeBuilder<CatalogRuleCustomerGroup> builder)
    {
        builder.ToTable("CatalogRuleCustomerGroups", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CatalogRuleId)
            .IsRequired();

        builder.Property(x => x.CustomerGroupId)
            .IsRequired();

        builder.HasIndex(x => new { x.CatalogRuleId, x.CustomerGroupId })
            .IsUnique();

        builder.HasOne(x => x.CatalogRule)
            .WithMany(x => x.CustomerGroups)
            .HasForeignKey(x => x.CatalogRuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
