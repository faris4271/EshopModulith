using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistence;


namespace Shared.Data
{
    public static class Extention
    {
        public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            MigratDataBaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

            return app;
        }

        private static async Task MigratDataBaseAsync<TContext>(IServiceProvider service) where TContext : DbContext
        {
            using var scope = service.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await seeder.SeedAsync();
            await context.Database.MigrateAsync();

        }
    }
}
