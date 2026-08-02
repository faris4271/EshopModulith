using Microsoft.EntityFrameworkCore;

namespace Module.Tax.Data
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaxDbContext).Assembly);
        }
    }
}
