using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category.Models.Category> Categories { get; set; }


        public CatalogDbContext(DbContextOptions options) : base(options)
        {
        }
        public CatalogDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("catalog");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogModule).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
