using Eshop.Module.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Module.Core.Data
{
    public class CoreDbContext : DbContext
    {
        DbSet<Entity> entities;
        DbSet<Media> media;
        public CoreDbContext(DbContextOptions options) : base(options)
        {
        }




    }
}
