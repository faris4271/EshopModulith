using Shared.Contract.CQRS;

public sealed record CompleteCheckoutCommand(
    Guid CheckoutId) : ICommand<CompleteCheckoutResponse>;

public sealed record CompleteCheckoutResponse(Guid OrderId);