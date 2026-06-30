
using Microsoft.EntityFrameworkCore;
using Module.Inventory.Models;
using Shared.Eventing;
using Shared.Eventing.Inbox;

namespace Module.Inventory.Data
{
    public sealed class InventoryDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }
        public DbSet<ProductBackInStockSubscription> ProductBackInStockSubscriptions { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("inventory");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);



        }
    }
}
