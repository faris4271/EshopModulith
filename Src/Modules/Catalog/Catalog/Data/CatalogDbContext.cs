using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category.Models.Category> Categories { get; set; }


        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("catalog");
            // Inside CatalogDbContext.cs or your EntityTypeConfiguration
            modelBuilder.Entity<Catalog.Products.Models.ProductAttributeValue>(entity =>
            {
                // Ensure the FK is explicitly defined
                entity.HasOne(e => e.Attribute)
                      .WithMany(a => a.AttributeValues)
                      .HasForeignKey(e => e.AttributeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // This tells EF Core to ignore the navigation property 'Attribute' 
                // for relationship fix-up if it's not being explicitly tracked.
                entity.Navigation(e => e.Attribute).AutoInclude(false);
            });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogModule).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
