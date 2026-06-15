using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eshop.Module.Basket.Data.Configurations
{
    internal class CartRuleCategoryConfiguration : IEntityTypeConfiguration<CartRuleCategory>
    {
        public void Configure(EntityTypeBuilder<CartRuleCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.CartRule).WithMany(x => x.Categories).HasForeignKey(x => x.CategoryId);
        }
    }
}
