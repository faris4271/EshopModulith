using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Basket.Data.Configurations
{
    internal class CartRuleProductConfiguration : IEntityTypeConfiguration<CartRuleProduct>
    {
        public void Configure(EntityTypeBuilder<CartRuleProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.CartRule).WithMany(x => x.Products).HasForeignKey(x => x.ProductId);
        }
    }
}
