using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Eshop.Module.Checkout.Contract.Dtos;
using Eshop.Module.Checkout.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

public sealed class ProceedToCheckoutHandler(
    IGenericeRepository<CartItem, BasketDbContext> basketRepository,
    ICheckoutService checkoutService) : ICommandHandler<ProceedToCheckoutCommand, ProceedToCheckoutResponse>
{
    async Task<Result<ProceedToCheckoutResponse>> IRequestHandler<ProceedToCheckoutCommand, Result<ProceedToCheckoutResponse>>.Handle(ProceedToCheckoutCommand request, CancellationToken cancellationToken)
    {
        var query = await basketRepository.GetAllAsQuerable();
        var basketItems = await query
            .Where(i => i.CustomerId == request.UserId.ToString())
            .Select(i => new CartItemToCheckoutDto(i.ProductId, i.Quantity))
            .ToListAsync(cancellationToken);

        if (!basketItems.Any())
        {
            return Result.Failure<ProceedToCheckoutResponse>(Error.NotFound("Basket.NotFound", "Basket was not found."));
        }

        var checkout = await checkoutService.Create(
            request.UserId,
            request.UserId,
            basketItems,
            request.CouponCode);

        return new ProceedToCheckoutResponse(checkout.Id);
    }
}
