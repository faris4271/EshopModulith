using IdentityModule.Domain;
using Microsoft.EntityFrameworkCore;

namespace IdentityModule.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }

      public DbSet<UserGroup> userGroups { set; get; } 

      public DbSet<GroupRole> groupRoles { set; get; }

      public DbSet<PasswordHistory> passwordHistory { set; get; }
      public DbSet<RoleClaim> roleClaim { set; get; }


    }
}
