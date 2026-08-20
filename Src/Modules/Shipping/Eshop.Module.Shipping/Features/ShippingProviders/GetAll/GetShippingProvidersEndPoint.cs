using Carter;
using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.GetAll;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.ShippingProviders.GetAll;

internal sealed class GetShippingProvidersEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/shipping-providers", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetShippingProvidersQuery(), ct);
            return result.Match(Results.Ok, Results.BadRequest);
        })
        .WithName(nameof(GetShippingProvidersQuery))
        .WithSummary("Get all shipping providers")
        .AllowAnonymous();
    }
}
