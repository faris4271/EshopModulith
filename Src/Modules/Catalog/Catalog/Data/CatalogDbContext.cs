using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Eventing;

namespace Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<Category.Models.Category> Categories { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("catalog");
            modelBuilder.Entity<Catalog.Products.Models.ProductAttributeValue>(entity =>
            {
                entity.HasOne(e => e.Attribute)
                      .WithMany(a => a.AttributeValues)
                      .HasForeignKey(e => e.AttributeId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Navigation(e => e.Attribute).AutoInclude(false);
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogModule).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
