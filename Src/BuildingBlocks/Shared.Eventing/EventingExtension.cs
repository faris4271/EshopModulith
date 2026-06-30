using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Eventing.Contract;
using Shared.Eventing.Inbox;
using Shared.Eventing.InMemory;
using Shared.Eventing.OutBox;
using Shared.Eventing.RabbitMq;
using Shared.Eventing.Serialization;
using System.Reflection;

namespace Shared.Eventing;

/// <summary>
/// Marker to track which assemblies contain integration event handlers,
/// so the RabbitMQ consumer can discover them at runtime.
/// </summary>
public sealed class HandlerAssemblyMarker
{
    public required Assembly Assembly { get; init; }
}

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core eventing services (serializer, bus, options).
    /// </summary>
    public static IServiceCollection AddEventingCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<EventingOptions>().BindConfiguration(nameof(EventingOptions));

        services.AddSingleton<IEventSerializer, JsonEventSerializer>();

        // Register event bus based on configured provider
        var options = configuration.GetSection(nameof(EventingOptions)).Get<EventingOptions>() ?? new EventingOptions();

        if (string.Equals(options.Provider, "RabbitMQ", StringComparison.OrdinalIgnoreCase))
        {
            services.AddOptions<RabbitMqOptions>().BindConfiguration("EventingOptions:RabbitMQ");
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            // Register the RabbitMQ consumer with explicit handler assemblies
            services.AddSingleton<RabbitMqConsumerHostedService>(sp =>
            {
                var handlerAssemblies = sp.GetServices<HandlerAssemblyMarker>().Select(h => h.Assembly);
                return new RabbitMqConsumerHostedService(
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqOptions>>(),
                    sp.GetRequiredService<IEventSerializer>(),
                    sp.GetRequiredService<ILogger<RabbitMqConsumerHostedService>>(),
                    handlerAssemblies);
            });
            services.AddHostedService(sp => sp.GetRequiredService<RabbitMqConsumerHostedService>());
        }
        else
        {
            // Default to InMemory
            services.AddSingleton<IEventBus, InMemoryEventBus>();
        }

        // Register outbox dispatcher hosted service if enabled
        if (options.UseHostedServiceDispatcher)
        {
            services.AddHostedService<OutboxDispatcherHostedService>();
        }

        return services;
    }

    /// <summary>
    /// Registers EF Core-based outbox and inbox stores for the specified DbContext.
    /// </summary>
    public static IServiceCollection AddEventingForDbContext<TDbContext>(
        this IServiceCollection services)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IOutBoxStore, EfCoreOutboxStore<TDbContext>>();
        services.AddScoped<IInboxStore, EfCoreInboxStore<TDbContext>>();
        services.AddScoped<OutboxDispatcher>();

        return services;
    }

    /// <summary>
    /// Registers integration event handlers from the specified assemblies.
    /// Also registers a HandlerAssemblyMarker for each assembly so the
    /// RabbitMQ consumer can discover handlers at runtime.
    /// </summary>
    public static IServiceCollection AddIntegrationEventHandlers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        if (assemblies is null || assemblies.Length == 0)
        {
            return services;
        }

        foreach (var assembly in assemblies)
        {
            // Register a marker so the RabbitMQ consumer knows which assemblies to scan
            services.AddSingleton(new HandlerAssemblyMarker { Assembly = assembly });

            var handlerTypes = assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Type = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))
                        .ToArray()
                })
                .Where(x => x.Interfaces.Length > 0);

            foreach (var handler in handlerTypes)
            {
                foreach (var handlerInterface in handler.Interfaces)
                {
                    services.AddScoped(handlerInterface, handler.Type);
                }
            }
        }

        return services;
    }
}