using Carter;
using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.EnableShippingProvider;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eshop.Module.Shipping.Features.ShippingProviders.EnableShippingProvider;

internal sealed class EnableShippingProviderEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/shipping-providers/{id}/enable", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new EnableShippingProviderCommand(id), ct);
            return result.Match(Results.NoContent, Results.BadRequest);
        })
        .WithName(nameof(EnableShippingProviderCommand))
        .WithSummary("Enable a shipping provider")
        .AllowAnonymous();
    }
}
