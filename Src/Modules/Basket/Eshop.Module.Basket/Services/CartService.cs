using CatalogContract.Services;
using Eshop.Module.Basket.Contract.Feature.GetProductsByIds;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Services;
using Shared.Abstraction;
using Shared.Caching;
using AddToCartResult = Eshop.Module.Basket.Contract.Dtos.AddToCartResult;
using CartDto = Eshop.Module.Basket.Contract.Dtos.CartDto;
using CartInfoForCoupon = Eshop.Module.Pricing.Contract.Dtos.CartInfoForCoupon;
using CartItemDto = Eshop.Module.Basket.Contract.Dtos.CartItemDto;
using CartItemForCoupon = Eshop.Module.Pricing.Contract.Dtos.CartItemForCoupon;
using CartQuantityUpdateDto = Eshop.Module.Basket.Contract.Dtos.CartQuantityUpdateDto;
using UpdateCartResult = Eshop.Module.Basket.Contract.Dtos.UpdateCartResult;

namespace Eshop.Module.Basket.Services
{
    internal class CartService(IGenericeRepository<CartItem, BasketDbContext> _repository,
        IProductPricingService _productPricing,
        ICurrentUserService _currentUser,
        ICouponService _couponService,
        ICurrencyService _currencyService,
        ICurrentUserService _currentUserService,
        ICacheService _cacheService,
        ISender sender
        ) : ICartService
    {
        public async Task<AddToCartResult> AddToCart(Guid customerId, Guid productId, int quantity)
        {
            var user = await _currentUser.GetCurrentUser();

            var addToCartResult = new AddToCartResult { Success = false };

            if (quantity <= 0)
            {
                addToCartResult.ErrorMessage = "The quantity must be larger than zero";
                addToCartResult.ErrorCode = "wrong-quantity";
                return addToCartResult;
            }



            var query = await _repository.Query();

            var cartItems = query.FirstOrDefault(x => x.CustomerId == user.Id && x.ProductId == productId);

            if (cartItems == null)
            {
                cartItems = new CartItem
                {

                    CustomerId = user.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedOn = DateTimeOffset.UtcNow,
                    LatestUpdatedOn = DateTimeOffset.UtcNow
                };
                await _repository.AddAsync(cartItems);
            }
            else
            {
                cartItems.Quantity += quantity;
                cartItems.LatestUpdatedOn = DateTimeOffset.UtcNow;
                _repository.Update(cartItems);
            }

            return addToCartResult;
        }

        public async Task<CouponValidationResult> ApplyCoupon(Guid customerId, string couponCode)
        {
            var query = await _repository.GetAllAsQuerable();

            var cartItem = await query.Where(x => x.CustomerId == customerId.ToString()).ToListAsync();

            var cartInfoForCoupon = new CartInfoForCoupon
            {
                Items = cartItem.Select(x => new CartItemForCoupon
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var validation = await _couponService.Validate(customerId, couponCode, cartInfoForCoupon);
            return validation;
        }

        public async Task<CartDto> GetCartDetails(Guid customerId, CancellationToken cancellationToken)
        {
            var cacheKey = GetCartCacheKey(customerId.ToString());


            var cartDto = await _cacheService.GetOrSetAsync(cacheKey, async () =>
            {
                var query = await _repository.Query();

                var productIds = query
                    .Where(x => x.CustomerId == customerId.ToString())
                    .Select(x => x.ProductId)
                    .ToList();

                var freshCartDto = new CartDto();

                if (!productIds.Any())
                {
                    return freshCartDto;
                }


                var productResult = await sender.Send(new GetProductsByIdsQuery(productIds), cancellationToken);
                var cartItems = productResult.Value ?? new List<CartItemDto>();

                freshCartDto.Items = cartItems;


                return freshCartDto;
            }, cancellationToken: cancellationToken);

            cartDto.SubTotal = cartDto.Items.Sum(x => x.Quantity * (x.CalculatedProductPrice.OldPrice?.Amount ?? x.CalculatedProductPrice.CalculatedPrice.Amount));

            cartDto.Discount = 0;

            if (!string.IsNullOrWhiteSpace(cartDto.CouponCode))
            {
                var cartInfoForCoupon = new CartInfoForCoupon
                {
                    Items = cartDto.Items.Select(x => new CartItemForCoupon { ProductId = x.ProductId, Quantity = x.Quantity }).ToList()
                };

                var couponValidationResult = await _couponService.Validate(customerId, cartDto.CouponCode, cartInfoForCoupon);

                if (couponValidationResult.Succeeded)
                {
                    cartDto.Discount = couponValidationResult.DiscountAmount;
                }
                else
                {
                    cartDto.CouponValidationErrorMessage = couponValidationResult.ErrorMessage;
                }
            }

            cartDto.Discount += cartDto.Items
                .Where(x => x.CalculatedProductPrice.OldPrice.IsPositive)
                .Sum(x => x.Quantity * (x.CalculatedProductPrice.OldPrice.Amount - x.CalculatedProductPrice.CalculatedPrice.Amount));

            return cartDto;
        }
        public async Task MigrateCart(Guid fromUserId, Guid toUserId)
        {
            var query = await _repository.Query();

            var cartItemsFrom = await query.Where(x => x.CustomerId == fromUserId.ToString()).ToListAsync();
            var cartItemsTo = await query.Where(x => x.CustomerId == toUserId.ToString()).ToListAsync();

            foreach (var cartItem in cartItemsFrom)
            {
                var existingCartItem = cartItemsTo.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartItem.Quantity;
                    existingCartItem.LatestUpdatedOn = DateTimeOffset.UtcNow;
                    _repository.Update(existingCartItem);
                    _repository.Delete(cartItem);
                }
                else
                {
                    cartItem.CustomerId = toUserId.ToString();
                    cartItem.LatestUpdatedOn = DateTimeOffset.UtcNow;
                    _repository.Update(cartItem);
                }
            }

            await _repository.SaveChangesAsync();
        }

        public async Task RemoveItem(Guid customerId, Guid productId)
        {
            var query = await _repository.Query();

            var cartItem = query.FirstOrDefault(
                x => x.CustomerId == customerId.ToString() &&
                x.ProductId == productId);

            if (cartItem != null)
            {
                _repository.Delete(cartItem);
            }


        }

        public async Task<UpdateCartResult> UpdateQuantity(CartQuantityUpdateDto cartQuantityUpdate)
        {
            var UpdateResult = new UpdateCartResult { Success = false };
            var cartItem = await _repository.GetByIdAsync(cartQuantityUpdate.cartItemId);

            var productId = new List<Guid>();
            productId.Add(cartQuantityUpdate.cartItemId);

            var cartItems = await sender.Send(new GetProductsByIdsQuery(productId));

            if (cartItems.Value.FirstOrDefault().
                ProductStockTrackingIsEnabled &&
                cartItems.Value.FirstOrDefault().
                ProductStockQuantity < cartQuantityUpdate.Quantity)
            {
                UpdateResult.ErrorMessage = $"Only {cartItems.Value.FirstOrDefault().ProductStockQuantity} items are available in stock.";
                return UpdateResult;
            }


            cartItem.Quantity = cartQuantityUpdate.Quantity;
            _repository.Update(cartItem);

            UpdateResult.Success = true;
            return UpdateResult;
        }


        public static string GetCartCacheKey(string userId)
        {
            return $"cart:{userId}";
        }
    }
}