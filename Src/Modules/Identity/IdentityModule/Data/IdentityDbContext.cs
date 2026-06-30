using IdentityModule.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Eventing;

namespace IdentityModule.Data
{
    public class IdentityDbContext : IdentityDbContext<AppUser, Role, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }


        public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();


        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<UserSession> UserSessions => Set<UserSession>();

        public DbSet<Group> Groups => Set<Group>();

        public DbSet<GroupRole> GroupRoles => Set<GroupRole>();

        public DbSet<UserGroup> UserGroups => Set<UserGroup>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            const string schema = "identity";
            modelBuilder.HasDefaultSchema(schema);




            modelBuilder.Entity<AppUser>().ToTable("AspNetUsers", schema);
            modelBuilder.Entity<Role>().ToTable("AspNetRoles", schema);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(u => u.NormalizedUserName).IsUnique(false);
            });


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        }


    }
}
