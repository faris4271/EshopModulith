using Eshop.Module.Checkout.Contract.Services;
using Eshop.Module.Checkout.Data;
using Eshop.Module.Checkout.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.interceptores;

namespace Eshop.Module.Checkout;

public static class CheckoutExtensions
{
    public static IServiceCollection AddCheckoutModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IGenericeRepository<,>), typeof(GenericeRepository<,>));
        services.AddDbContext<CheckoutDbContext>((sp, option) =>
        {
            var domainEventInterceptor = sp.GetRequiredService<DispachDomainEventInterceptor>();
            var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            option.AddInterceptors(domainEventInterceptor).AddInterceptors(auditInterceptor);


            option.UseNpgsql(configuration.GetConnectionString("defualt"));
        });

        services.AddScoped<ICheckoutService, CheckoutService>();
        return services;
    }
}
