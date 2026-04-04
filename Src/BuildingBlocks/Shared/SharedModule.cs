using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared
{
    public static class SharedModule
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {
            services.AddScoped<IFileService, LocalFileService>();

            return services;
        }
    }
}
