using IdentityModule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DDD;

namespace IdentityModule.Data.Configurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(u => u.UserAddresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.DefaultShippingAddress)
                .WithMany()
                .HasForeignKey(u => u.DefaultShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(u => u.DefaultBillingAddress)
                .WithMany()
                .HasForeignKey(u => u.DefaultBillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.FirstName).HasConversion(
                v => v.name,
                v => v == null ? null : new Name(v)
            );

            builder.Property(x => x.LastName).HasConversion(
                v => v.name,
                v => v == null ? null : new Name(v)
            );




        }
    }
}
