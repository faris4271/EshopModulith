using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Basket.Data.Configurations
{
    internal class CatalogRuleCustomerGroupConfiguration : IEntityTypeConfiguration<CatalogRuleCustomerGroup>
    {
        public void Configure(EntityTypeBuilder<CatalogRuleCustomerGroup> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.CatalogRule).WithMany(x => x.CustomerGroups).HasForeignKey(x => x.CustomerGroupId);
        }
    }
}
