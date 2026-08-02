using Shared.Contract.CQRS;

public sealed record CreateCheckoutCommand(
    Guid BasketId,
    Guid UserId,
    decimal TotalAmount) : ICommand<CreateCheckoutResponse>;

public sealed record CreateCheckoutResponse(Guid CheckoutId);