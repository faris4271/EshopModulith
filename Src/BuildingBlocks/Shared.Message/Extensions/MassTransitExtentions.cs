//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;

//public static class MassTransitExtentions
//{
//    public static IServiceCollection AddModularMassTransit(
//        this IServiceCollection services,
//        IConfiguration configuration,
//        Action<IBusRegistrationConfigurator> configureModules)
//    {
//        services.AddMassTransit(config =>
//        {
//            config.SetKebabCaseEndpointNameFormatter();


//            configureModules(config);

//            config.UsingRabbitMq((context, configurator) =>
//            {
//                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
//                {
//                    host.Username(configuration["MessageBroker:UserName"]!);
//                    host.Password(configuration["MessageBroker:Password"]!);
//                });

//                configurator.ConfigureEndpoints(context);
//            });
//        });

//        return services;
//    }

//    public static void AddModuleMassTransit<TDbContext>(
//        this IBusRegistrationConfigurator configurator,
//        params Assembly[] assemblies) where TDbContext : DbContext
//    {
//        configurator.AddConsumers(assemblies);
//        configurator.AddSagaStateMachines(assemblies);
//        configurator.AddSagas(assemblies);
//        configurator.AddActivities(assemblies);

//        configurator.AddEntityFrameworkOutbox<TDbContext>(o =>
//        {
//            o.UsePostgres();
//            o.UseBusOutbox();
//        });
//    }
//}