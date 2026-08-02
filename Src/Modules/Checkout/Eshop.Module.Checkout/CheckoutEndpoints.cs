public sealed class CheckoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/checkout")
            .WithTags("Checkout");
        // Map feature endpoints here
    }
}