using Carter;
using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.DisableShippingProvider;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.ShippingProviders.DisableShippingProvider;

internal sealed class DisableShippingProviderEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/shipping-providers/{id}/disable", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DisableShippingProviderCommand(id), ct);
            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithName(nameof(DisableShippingProviderCommand))
        .WithSummary("Disable a shipping provider")
        .AllowAnonymous();
    }
}
