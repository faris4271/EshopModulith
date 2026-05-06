using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Module.Identity.Contract.Services;
using Shared.Context;


namespace Eshop.Module.Localization
{
    public class EfRequestCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var workContext = httpContext.RequestServices.GetRequiredService<ICurrentUserService>();
            var user = await workContext.GetCurrentUser();
            var culture = user.Culture;

            if (culture == null)
            {
                return await Task.FromResult((ProviderCultureResult)null);
            }

            var providerResultCulture = new ProviderCultureResult(culture);

            return await Task.FromResult(providerResultCulture);
        }
    }
}