using Eshop.Module.Pricing.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DDD;

internal sealed class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("ExchangeRates", "pricing");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FromCurrency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.ToCurrency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Rate)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(x => x.LastUpdatedUtc)
            .IsRequired();

        // Global query filter for soft delete
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}