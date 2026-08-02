using Eshop.Module.Checkout.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.interceptores;

namespace Eshop.Module.Checkout;

public static class CheckoutExtensions
{
    public static IServiceCollection AddCheckoutModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CheckoutDbContext>((sp, option) =>
        {
            var domainEventInterceptor = sp.GetRequiredService<DispachDomainEventInterceptor>();
            var auditInterceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            option.AddInterceptors(domainEventInterceptor).AddInterceptors(auditInterceptor);


            option.UseNpgsql(configuration.GetConnectionString("defualt"));
        });
        return services;
    }
}
