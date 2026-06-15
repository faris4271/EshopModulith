using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Eventing.Contract;
using Shared.Eventing.OutBox;
using Shared.Eventing.RabbitMq;
using Shared.Eventing.Serialization;
using Shared.Message.OutBox;
using System.Reflection;

namespace Shared.Eventing
{
    public static class EventingExtension
    {
        public static IServiceCollection AddEventingCore(this IServiceCollection services, IConfiguration configuration)
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
            }
            //else
            //{
            //    // Default to InMemory
            //    services.AddSingleton<IEventBus, InMemoryEventBus>();
            //}

            // Register outbox dispatcher hosted service if enabled
            if (options.UseHostedServiceDispatcher)
            {
                services.AddHostedService<OutboxDispatcherHostedService>();
            }

            return services;
        }

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

        public static IServiceCollection AddEventingForDbContext<TDbContext>(
                  this IServiceCollection services)
                   where TDbContext : DbContext
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<IOutBoxStore, EfCoreOutboxStore<TDbContext>>();
            services.AddScoped<OutboxDispatcher>();

            return services;
        }

    }
}
