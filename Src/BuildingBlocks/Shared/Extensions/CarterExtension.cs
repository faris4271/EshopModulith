using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions
{
    public static class CarterExtension
    {

        public static IServiceCollection AddCarterWithAssembly(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddCarter(configurator: config =>
            {
                foreach (var assembly in assemblies)
                {
                    var module = assembly.GetTypes().
                    Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

                    config.WithModules(module);
                }
            });
            return services;
        }
    }
}
