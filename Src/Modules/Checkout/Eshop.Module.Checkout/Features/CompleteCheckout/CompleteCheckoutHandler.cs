using Eshop.Module.Checkout.Data;
using Shared.Contract.Exeption;

public sealed class CompleteCheckoutHandler(
    IGenericeRepository<Checkout, CheckoutDbContext> checkoutRepository,
    IGenericeRepository<Order, CheckoutDbContext> orderRepository) : ICommandHandler<CompleteCheckoutCommand, CompleteCheckoutResponse>
{


    async Task<Result<CompleteCheckoutResponse>> IRequestHandler<CompleteCheckoutCommand, Result<CompleteCheckoutResponse>>.Handle(CompleteCheckoutCommand request, CancellationToken cancellationToken)
    {
        var checkout = await checkoutRepository.GetByIdAsync(request.CheckoutId, cancellationToken);
        if (checkout == null)
        {
            throw new CustomException(nameof(Checkout));
        }

        var order = Order.Create(checkout.Id, Guid.NewGuid().ToString(), checkout.TotalAmount);
        await orderRepository.AddAsync(order, cancellationToken);
        checkout.UpdateStatus("Completed");
        await checkoutRepository.UpdateAsync(checkout, cancellationToken);
        return new CompleteCheckoutResponse(order.Id);
    }
}