using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Inventory.Models;
using Shared.DDD;

namespace Module.Inventory.Data.Configurations
{
    internal class WarehousConfiguration : IEntityTypeConfiguration<Models.Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasConversion(
                v => v.name,
                v => new Name(v)
            ).HasMaxLength(100).IsRequired();
            builder.OwnsOne(x => x.Address, a =>
            {
                a.Property(p => p.Street).HasMaxLength(200).IsRequired();
                a.Property(p => p.City).HasMaxLength(100).IsRequired();
                a.Property(p => p.State).HasMaxLength(100).IsRequired();
                a.Property(p => p.ZipCode).HasMaxLength(20).IsRequired();
                a.Property(p => p.Country).HasMaxLength(100).IsRequired();
            });
        }
    }
}
