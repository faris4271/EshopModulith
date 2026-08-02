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

            builder.HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ua => ua.UserId);
            builder.HasIndex(ua => ua.AddressId);
        }
    }
}