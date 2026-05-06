using CatalogContract.Services;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Module.Identity.Contract.Services;
using Shared.Abstraction;
using Shared.Context;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Basket.Feature.Commands.AddItems
{
    internal class AddToBasketCommandHandler(
        IGenericeRepository<CartItem,BasketDbContext> _repository,
        IProductPricingService _productPricing,
        ICurrentUserService _currentUser
        ) 
        : ICommandHandler<AddToBasketCommand,string>
    {
        public async Task<Result<string>> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
        {
            var user = await _currentUser.GetCurrentUser();

            

            if (request.quantity <= 0)
            {
                return Result.Failure<string>(Error.Problem("404", "Quantity must be greater than zero."));
            }

            if(request.productId == Guid.Empty)
            {
                return Result.Failure<string>(Error.Problem("404", "ProductId is required."));
            }

            var query =await _repository.Query();

            var cartItems=query.FirstOrDefault(x => x.CustomerId == user.Id && x.ProductId == request.productId);

            if (cartItems == null)
            {
                cartItems = new CartItem
                {
                    
                    CustomerId = user.Id,
                    ProductId = request.productId,
                    Quantity = request.quantity,
                    CreatedOn = DateTimeOffset.UtcNow,
                    LatestUpdatedOn = DateTimeOffset.UtcNow
                };
                await _repository.AddAsync(cartItems);
            }
            else
            {
                cartItems.Quantity += request.quantity;
                cartItems.LatestUpdatedOn = DateTimeOffset.UtcNow;
                 _repository.Update(cartItems);   
            }

            return Result.Success(cartItems.Id.ToString());
        }
    }
}
