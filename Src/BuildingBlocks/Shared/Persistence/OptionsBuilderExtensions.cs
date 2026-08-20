using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shared.Persistence;

/// <summary>
/// Extension methods for configuring Entity Framework DbContextOptionsBuilder.
/// </summary>
public static class OptionsBuilderExtensions
{
    /// <summary>
    /// Configures the database provider and connection for the Hero framework.
    /// </summary>
    public static DbContextOptionsBuilder ConfigureHeroDatabase(
        this DbContextOptionsBuilder builder,
        string dbProvider,
        string connectionString,
        string? migrationsAssembly,
        bool isDevelopment)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(dbProvider);

        builder.ConfigureWarnings(warnings =>
            warnings.Log(RelationalEventId.PendingModelChangesWarning));


        bool hasMigrationsAssembly = !string.IsNullOrWhiteSpace(migrationsAssembly);

        switch (dbProvider.ToUpperInvariant())
        {
            case DbProviders.PostgreSQL:
                builder.UseNpgsql(connectionString, e =>
                {
                    if (hasMigrationsAssembly)
                    {
                        e.MigrationsAssembly(migrationsAssembly);
                    }
                });
                break;

            case DbProviders.MSSQL:
                builder.UseSqlServer(connectionString, e =>
                {
                    if (hasMigrationsAssembly)
                    {
                        e.MigrationsAssembly(migrationsAssembly);
                    }
                    e.EnableRetryOnFailure();
                });
                break;

            default:
                throw new InvalidOperationException(
                    $"Database Provider {dbProvider} is not supported.");
        }

        if (isDevelopment)
        {
            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();
        }

        return builder;
    }
}