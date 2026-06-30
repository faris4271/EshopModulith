using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Inventory.Models;

namespace Module.Inventory.Data.Configurations
{
    internal class ProductBackInStockSubscriptionConfiguration : IEntityTypeConfiguration<ProductBackInStockSubscription>
    {
        public void Configure(EntityTypeBuilder<ProductBackInStockSubscription> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductId)
                .IsRequired();
            builder.Property(x => x.CustomerEmail).IsRequired();
        }
    }
}
