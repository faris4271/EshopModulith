using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CatalogRuleConfiguration : IEntityTypeConfiguration<CatalogRule>
{
    public void Configure(EntityTypeBuilder<CatalogRule> builder)
    {
        builder.ToTable("CatalogRules", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.Description);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.StartOn);

        builder.Property(x => x.EndOn);

        builder.Property(x => x.RuleToApply)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.MaxDiscountAmount)
            .HasPrecision(18, 4);

        builder.HasMany(x => x.CustomerGroups)
            .WithOne(x => x.CatalogRule)
            .HasForeignKey(x => x.CatalogRuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
