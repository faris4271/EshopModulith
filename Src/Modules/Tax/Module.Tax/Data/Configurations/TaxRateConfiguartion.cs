using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Tax.Models;

namespace Module.Tax.Data.Configurations
{
    internal class TaxRateConfiguartion : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TaxClass)
                .WithMany()
                .HasForeignKey(x => x.TaxClassId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
