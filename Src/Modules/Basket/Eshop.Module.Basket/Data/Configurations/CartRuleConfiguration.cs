using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Basket.Data.Configurations
{
    internal class CartRuleConfiguration : IEntityTypeConfiguration<CartRule>
    {
        public void Configure(EntityTypeBuilder<CartRule> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasMany(x => x.Coupons).WithOne(x => x.CartRule).HasForeignKey(x => x.CartRuleId);
        }
    }
}
