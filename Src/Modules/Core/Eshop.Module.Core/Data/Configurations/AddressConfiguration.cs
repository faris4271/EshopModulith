using Eshop.Module.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Core.Data.Configurations
{
    internal class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses", "Core");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ContactName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.AddressLine1)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.AddressLine2)
                .HasMaxLength(500);

            builder.Property(x => x.CityName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ZipCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.CountryId)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.StateOrProvinceId)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.DistrictId)
                .HasMaxLength(10);

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Property(x => x.CreatedById)
                .HasMaxLength(450);

            builder.Property(x => x.LatestUpdatedOn);

            builder.Property(x => x.LatestUpdatedById)
                .HasMaxLength(450);

            builder.HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId)
                .HasPrincipalKey(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.StateOrProvince)
                .WithMany()
                .HasForeignKey(a => a.StateOrProvinceId)
                .HasPrincipalKey(s => s.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.District)
                .WithMany()
                .HasForeignKey(a => a.DistrictId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.CountryId);
            builder.HasIndex(x => x.StateOrProvinceId);
            builder.HasIndex(x => x.DistrictId);
            builder.HasIndex(x => x.CityName);
            builder.HasIndex(x => x.ZipCode);
        }
    }
}