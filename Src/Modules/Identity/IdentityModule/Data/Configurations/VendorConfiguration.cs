using IdentityModule.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.DDD;

namespace IdentityModule.Data.Configurations
{
    internal class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasMany(x => x.Users).WithOne()
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).HasConversion(
                v => v.name,
                v => new Name(v)
                );

            builder.Property(x => x.Description).HasConversion(
                v => v.description,
                v => v == null ? null : new Description(v)
            );

            builder.Property(x => x.Email).HasConversion(
                v => v.email,
                v => new Email(v)
            );

        }
    }
}
