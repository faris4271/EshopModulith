using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Pricing.Data.Configurations;

internal sealed class CartRuleCustomerGroupConfiguration : IEntityTypeConfiguration<CartRuleCustomerGroup>
{
    public void Configure(EntityTypeBuilder<CartRuleCustomerGroup> builder)
    {
        builder.ToTable("CartRuleCustomerGroups", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CartRuleId)
            .IsRequired();

        builder.Property(x => x.CustomerGroupId)
            .IsRequired();

        builder.HasOne(x => x.CartRule)
            .WithMany(x => x.CustomerGroups)
            .HasForeignKey(x => x.CartRuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CartRuleId, x.CustomerGroupId })
            .IsUnique();
    }
}
