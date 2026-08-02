using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Checkout.Data.Configurations;

public sealed class CheckoutItemConfiguration : IEntityTypeConfiguration<Entities.CheckoutItem>
{
    public void Configure(EntityTypeBuilder<Entities.CheckoutItem> builder)
    {
        builder.ToTable("checkout_items", "checkout");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.CheckoutId)
            .IsRequired();

        // Assuming navigation properties will be added to Entities.Checkout and Entities.CheckoutItem
    }
}
