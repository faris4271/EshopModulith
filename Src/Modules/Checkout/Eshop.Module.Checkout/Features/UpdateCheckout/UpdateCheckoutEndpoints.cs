public sealed class UpdateCheckoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/v1/checkout/{id}", async (
            Guid id,
            UpdateCheckoutCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var updatedCommand = command with { CheckoutId = id };
            var result = await sender.Send(updatedCommand, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCheckoutCommand))
        .WithSummary("Update a checkout")
        .AllowAnonymous();
    }
}