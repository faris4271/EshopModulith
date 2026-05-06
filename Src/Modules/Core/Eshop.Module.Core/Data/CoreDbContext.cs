using Eshop.Module.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Core.Data
{
    public class CoreDbContext : DbContext
    {
       public DbSet<Entity> entities { get; set; }
       public DbSet<Media> media { get; set; }
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreModule).Assembly);

            modelBuilder.HasDefaultSchema("Core");
        }



    }
}
