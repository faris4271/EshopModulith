using Shared.Contract.Exeption;

public sealed class UpdateCheckoutHandler(
    IGenericeRepository<Checkout, CheckoutDbContext> repository) : ICommandHandler<UpdateCheckoutCommand, UpdateCheckoutResponse>
{


    async Task<Result<UpdateCheckoutResponse>> IRequestHandler<UpdateCheckoutCommand, Result<UpdateCheckoutResponse>>.Handle(UpdateCheckoutCommand request, CancellationToken cancellationToken)
    {
        var checkout = await repository.GetByIdAsync(request.CheckoutId, cancellationToken);
        if (checkout == null)
        {
            throw new CustomException(nameof(Checkout));
        }

        checkout.UpdateStatus(request.Status);
        await repository.UpdateAsync(checkout, cancellationToken);
        return new UpdateCheckoutResponse(checkout.Id);
    }
}