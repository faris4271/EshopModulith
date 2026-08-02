using Eshop.Module.Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Basket.Data
{
    public class BasketDbContext : DbContext
    {

        public DbSet<CartItem> CartItems { get; set; }




        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("basket");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketModule).Assembly);




        }

    }
}
