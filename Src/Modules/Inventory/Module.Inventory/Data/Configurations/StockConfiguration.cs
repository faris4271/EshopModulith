using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Inventory.Models;

namespace Module.Inventory.Data.Configurations
{
    internal class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductId)
                .IsRequired();
            builder.Property(x => x.WarehouseId)
                .IsRequired();
            builder.HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.Quantity)
                .IsRequired();
            builder.Property(x => x.ReservedQuantity).IsRequired();
            builder.HasIndex(x => x.ProductId).IsUnique(true);



        }
    }
}
