using IdentityModule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Data.Configurations
{
    internal class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(ua => ua.Id);

            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(250);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(100);
                address.Property(a => a.Country).HasMaxLength(100);
                address.Property(a => a.ZipCode).HasMaxLength(20);
                address.Property(a => a.Phone).HasMaxLength(30);
                address.Property(a => a.PostalCode).HasMaxLength(20);
            });

        }
    }
}
