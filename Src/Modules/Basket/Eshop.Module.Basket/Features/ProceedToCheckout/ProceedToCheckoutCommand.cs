using Shared.Contract.CQRS;

public sealed record ProceedToCheckoutCommand(
    Guid BasketId,
    Guid UserId,
    string CouponCode) : ICommand<ProceedToCheckoutResponse>;

public sealed record ProceedToCheckoutResponse(Guid CheckoutId);