public sealed class CompleteCheckoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/v1/checkout/{id}/complete", async (
            Guid id,
            ISender sender,
            CancellationToken ct) =>
        {
            var command = new CompleteCheckoutCommand(id);
            var result = await sender.Send(command, ct);
            return result.Match(
                success => Results.Ok(success),
                failure => Results.BadRequest(failure));
        })
        .WithName(nameof(CompleteCheckoutCommand))
        .WithSummary("Complete a checkout")
        .AllowAnonymous();
    }
}