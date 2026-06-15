using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Basket.Data
{
    internal class BasketDbContext : DbContext
    {

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<CartRule> CartRules { get; set; }

        public DbSet<CartRuleUsage> CartRuleUsages { get; set; }


        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("basket");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketModule).Assembly);

            base.OnModelCreating(modelBuilder);


        }

    }
}
