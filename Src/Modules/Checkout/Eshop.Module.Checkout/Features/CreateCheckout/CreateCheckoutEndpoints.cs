using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Contract.ResultPattern;

namespace YourNamespace.Features.Checkouts;

public sealed class CreateCheckoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/checkout", HandleAsync)
           .WithName("CreateCheckout")
           .WithSummary("Create a new checkout")
           .WithDescription("Initiates a new checkout session and returns the resource URI.")
           .Produces<Created<CreateCheckoutResponse>>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .AllowAnonymous();
    }

    private static async Task<Created<CreateCheckoutResponse>> HandleAsync(
        CreateCheckoutCommand command,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(command, ct);

        return result.Match(
            success => TypedResults.Created($"/api/checkout/{success.CheckoutId}", new CreateCheckoutResponse(success.CheckoutId)),
            failure => throw new InvalidOperationException($"Failed to create checkout: {failure}")
        );
    }
}