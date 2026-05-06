using IdentityModule.Domain;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Shared.Persistence;

namespace IdentityModule.Data
{
    public class IdentityDbContext : IdentityDbContext<AppUser, Role, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }




        private readonly DatabaseOptions _settings;
        private readonly IHostEnvironment _environment;
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();


        public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();

        public DbSet<UserSession> UserSessions => Set<UserSession>();

        public DbSet<Group> Groups => Set<Group>();

        public DbSet<GroupRole> GroupRoles => Set<GroupRole>();

        public DbSet<UserGroup> UserGroups => Set<UserGroup>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
            modelBuilder.HasDefaultSchema("Identity");
        }


    }
}
