using Eshop.Module.Checkout.Data;

public sealed class CreateCheckoutHandler(
    IGenericeRepository<Checkout, CheckoutDbContext> repository) : ICommandHandler<CreateCheckoutCommand, CreateCheckoutResponse>
{

    async Task<Result<CreateCheckoutResponse>> IRequestHandler<CreateCheckoutCommand, Result<CreateCheckoutResponse>>.Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
    {

        var checkout = Checkout.Create(request.BasketId, request.UserId, request.TotalAmount);

        await repository.AddAsync(checkout, cancellationToken);

        return new CreateCheckoutResponse(checkout.Id);
    }

}