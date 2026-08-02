using Eshop.Module.Shipping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Shipping.Data.Configurations;

internal sealed class ShippingProviderConfiguration : IEntityTypeConfiguration<ShippingProvider>
{
    public void Configure(EntityTypeBuilder<ShippingProvider> builder)
    {
        builder.ToTable("ShippingProviders");

        builder.HasKey(x => x.Id);

        builder.Property(c => c.Name)
            .HasConversion(a => a.name, name => new(name));

        builder.Property(x => x.IsEnabled);
        builder.Property(x => x.ConfigureUrl);
        builder.Property(x => x.ToAllShippingEnabledCountries);
        builder.Property(x => x.OnlyCountryIdsString).HasMaxLength(1000);
        builder.Property(x => x.ToAllShippingEnabledStatesOrProvinces);
        builder.Property(x => x.OnlyStateOrProvinceIdsString);
        builder.Property(x => x.AdditionalSettings);
        builder.Property(x => x.ShippingPriceServiceTypeName).HasMaxLength(450);

        builder.HasData(ShippingProvider.Create("TableRate", true, "shipping-table-rate-config", true, "SimplCommerce.Module.ShippingTableRate.Services.TableRateShippingServiceProvider,SimplCommerce.Module.ShippingTableRate", true));
        builder.HasData(ShippingProvider.Create("FreeShipping", true, "shipping-free-config", true, "SimplCommerce.Module.ShippingTableRate.Services.TableRateShippingServiceProvider,SimplCommerce.Module.ShippingFree", true));
    }
}
