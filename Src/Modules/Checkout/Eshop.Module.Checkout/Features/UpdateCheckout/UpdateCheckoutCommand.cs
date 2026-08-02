public sealed record UpdateCheckoutCommand(
    Guid CheckoutId,
    string Status) : ICommand<UpdateCheckoutResponse>;

public sealed record UpdateCheckoutResponse(Guid CheckoutId);