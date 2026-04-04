using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Shared.Message.Extensions
{
    public static class MassTransitExtentions
    {
        public static IServiceCollection AddMassTransitWithAssemblies<TDbContext>
            (this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies) where TDbContext : DbContext
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.SetInMemorySagaRepositoryProvider();

                config.AddEntityFrameworkOutbox<TDbContext>(x =>
                {
                    x.UsePostgres();
                    x.UseBusOutbox();
                    x.DuplicateDetectionWindow = TimeSpan.FromMilliseconds(10);

                });

                config.AddConsumers(assemblies);
                config.AddSagaStateMachines(assemblies);
                config.AddSagas(assemblies);
                config.AddActivities(assemblies);

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });
            return services;

        }
    }
}
