using Eshop.Module.Localization.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Localization.Data
{
    public class LocalizationDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {
        }


    }
}
