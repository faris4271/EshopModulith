using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Module.Tax.Models;

namespace Module.Tax.Data.Configurations
{
    internal class TaxClassConfiguartion : IEntityTypeConfiguration<Models.TaxClass>
    {
        public void Configure(EntityTypeBuilder<TaxClass> builder)
        {
            builder.HasKey(x => x.Id);


            builder.Property(c => c.Name).HasConversion(a => a.name, name => new(name));


        }
    }
}
