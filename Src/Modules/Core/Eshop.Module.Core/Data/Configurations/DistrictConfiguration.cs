using Eshop.Module.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Core.Data.Configurations
{
    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts", "Core");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(10)
                .ValueGeneratedNever();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.StateOrProvinceId)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasOne(x => x.StateOrProvince)
                .WithMany()
                .HasForeignKey(x => x.StateOrProvinceId)
                .HasPrincipalKey(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}