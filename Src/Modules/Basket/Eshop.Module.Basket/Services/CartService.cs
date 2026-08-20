using CatalogContract.Dtos;
using Eshop.Module.Basket.Contract.Feature.GetProductsByIds;
using Eshop.Module.Basket.Data;
using Eshop.Module.Basket.Models;
using Eshop.Module.Pricing.Contract.Dtos;
using Eshop.Module.Pricing.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Module.Identity.Contract.Services;
using AddToCartResult = Eshop.Module.Basket.Contract.Dtos.AddToCartResult;
using CartDto = Eshop.Module.Basket.Contract.Dtos.CartDto;
using CartInfoForCoupon = Eshop.Module.Pricing.Contract.Dtos.CartInfoForCoupon;
using CartItemForCoupon = Eshop.Module.Pricing.Contract.Dtos.CartItemForCoupon;
using CartQuantityUpdateDto = Eshop.Module.Basket.Contract.Dtos.CartQuantityUpdateDto;
using UpdateCartResult = Eshop.Module.Basket.Contract.Dtos.UpdateCartResult;

namespace Eshop.Module.Basket.Services
{
    internal class CartService(BasketDbContext _context,
        ICurrentUserService _currentUser,
        ICouponService _couponService,
        ICurrencyService _currencyService,
        BasketPricingService _basketPricing,
        HybridCache _cache,
        ISender sender
        ) : ICartService
    {

        private readonly record struct CartItemFactoryState(string cashKey, BasketDbContext Context, string CustomerId);

        private static readonly HybridCacheEntryOptions CartEntryOptions = new()
        {
            Expiration = TimeSpan.FromHours(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(2),
        };
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



            var query = _context.Set<CartItem>();

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
                await _context.AddAsync(cartItems);
            }
            else
            {
                cartItems.Quantity += quantity;
                cartItems.LatestUpdatedOn = DateTimeOffset.UtcNow;
                _context.Update(cartItems);
            }

            await _context.SaveChangesAsync();
            addToCartResult.Success = true;
            return addToCartResult;
        }

        public async Task<CouponValidationResult> ApplyCoupon(Guid customerId, string couponCode)
        {
            var query = _context.Set<CartItem>();

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

        public async Task<CartDto> GetCartDetails(Guid customerId, CancellationToken ct)
        {
            var cacheKey = GetCartCacheKey(customerId.ToString());

            var state = new CartItemFactoryState(cacheKey, _context, customerId.ToString());
            var tags = new[] { $"cart:{customerId}" };
            var cartDto = await _cache.GetOrCreateAsync(cacheKey,
                state,
                LoadCartDetailsFromDb, CartEntryOptions, tags, ct);

            cartDto.Discount = 0;

            if (!string.IsNullOrWhiteSpace(cartDto.CouponCode))
            {
                var cartInfoForCoupon = new CartInfoForCoupon
                {
                    Items = cartDto.Items.Select(x => new CartItemForCoupon { ProductId = x.Id, Quantity = x.StockQuantity }).ToList()
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



            return cartDto;
        }

        private async ValueTask<CartDto> LoadCartDetailsFromDb(CartItemFactoryState state, CancellationToken cancellationToken)
        {
            var query = _context.Set<CartItem>();
            var cartItems = await query.Where(x => x.CustomerId == state.CustomerId).ToListAsync(cancellationToken);
            var productIds = cartItems.Select(x => x.ProductId).ToList();
            var products = await sender.Send(new GetProductsByIdsQuery(productIds), cancellationToken);
            var items = cartItems.Select(cartItem =>
                {
                    var product = products.Value.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                    if (product == null)
                    {
                        return null;
                    }

                    var price = _basketPricing.CalculateItemPrice(
                        new Shared.DDD.Money(product.Price, "USD"),
                        product.ProductOldPrice != 0 ? new Shared.DDD.Money(product.ProductOldPrice, "USD") : null,
                        product.SpecialPrice != 0 ? new Shared.DDD.Money(product.SpecialPrice, "USD") : null,
                        product.SpecialPriceStart,
                        product.SpecialPriceEnd,
                        cartItem.Quantity,
                        product.TierPrices,
                        product.DiscountPercentage,
                        "bulk");

                    return new CartItemsDto
                    {
                        Id = product.ProductId,
                        Name = product.ProductName,
                        StockQuantity = cartItem.Quantity,
                        Price = price,
                    };
                })
                .OfType<CartItemsDto>()
                .ToList();

            var cartDto = new CartDto
            {
                Items = items,
                SubTotal = items.Sum(item => item!.Price * item.StockQuantity)
            };

            return cartDto;
        }

        public async Task MigrateCart(Guid fromUserId, Guid toUserId)
        {
            var query = _context.Set<CartItem>();

            var cartItemsFrom = await query.Where(x => x.CustomerId == fromUserId.ToString()).ToListAsync();
            var cartItemsTo = await query.Where(x => x.CustomerId == toUserId.ToString()).ToListAsync();

            foreach (var cartItem in cartItemsFrom)
            {
                var existingCartItem = cartItemsTo.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartItem.Quantity;
                    existingCartItem.LatestUpdatedOn = DateTimeOffset.UtcNow;
                    _context.Update(existingCartItem);
                    _context.Remove(cartItem);
                }
                else
                {
                    cartItem.CustomerId = toUserId.ToString();
                    cartItem.LatestUpdatedOn = DateTimeOffset.UtcNow;
                    _context.Update(cartItem);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItem(Guid customerId, Guid productId)
        {
            var query = _context.Set<CartItem>();

            var cartItem = query.FirstOrDefault(
                x => x.CustomerId == customerId.ToString() &&
                x.ProductId == productId);

            if (cartItem != null)
            {
                _context.Remove(cartItem);
                await _context.SaveChangesAsync();
            }


        }

        public async Task<UpdateCartResult> UpdateQuantity(CartQuantityUpdateDto cartQuantityUpdate)
        {
            var UpdateResult = new UpdateCartResult { Success = false };
            var cartItem = await _context.Set<CartItem>().FirstOrDefaultAsync(x => x.Id == cartQuantityUpdate.cartItemId);

            if (cartItem == null)
            {
                UpdateResult.ErrorMessage = "Cart item not found.";
                return UpdateResult;
            }

            var productId = new List<Guid> { cartItem.ProductId };

            var cartItems = await sender.Send(new GetProductsByIdsQuery(productId));

            var product = cartItems.Value.FirstOrDefault();

            if (product != null &&
                product.IsProductAvailabeToOrder &&
                product.ProductStockQuantity < cartQuantityUpdate.Quantity)
            {
                UpdateResult.ErrorMessage = $"Only {product.ProductStockQuantity} items are available in stock.";
                return UpdateResult;
            }


            cartItem.Quantity = cartQuantityUpdate.Quantity;
            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            UpdateResult.Success = true;
            return UpdateResult;
        }


        public static string GetCartCacheKey(string userId)
        {
            return $"cart:{userId}";
        }

        public async Task<decimal> CalculateBasketTotal(Guid customerId, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<CartItem>();
            var cartItems = await query.Where(x => x.CustomerId == customerId.ToString()).ToListAsync(cancellationToken);

            var productIds = cartItems.Select(x => x.ProductId).ToList();
            var products = await sender.Send(new GetProductsByIdsQuery(productIds), cancellationToken);

            decimal total = 0m;

            foreach (var cartItem in cartItems)
            {
                var product = products.Value.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                if (product == null)
                {
                    continue;
                }

                total += _basketPricing.CalculateItemPrice(
                    new Shared.DDD.Money(product.Price, "USD"),
                    product.ProductOldPrice != 0 ? new Shared.DDD.Money(product.ProductOldPrice, "USD") : null,
                    product.SpecialPrice != 0 ? new Shared.DDD.Money(product.SpecialPrice, "USD") : null,
                    product.SpecialPriceStart,
                    product.SpecialPriceEnd,
                    cartItem.Quantity,
                    product.TierPrices,
                    product.DiscountPercentage,
                    "bulk") * cartItem.Quantity;
            }

            // Apply basket-level coupon discount if one is set on the cart
            var cartInfoForCoupon = new CartInfoForCoupon
            {
                Items = cartItems.Select(x => new CartItemForCoupon
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var cartDto = await GetCartDetails(customerId, cancellationToken);
            if (!string.IsNullOrWhiteSpace(cartDto.CouponCode))
            {
                var validation = await _couponService.Validate(customerId, cartDto.CouponCode, cartInfoForCoupon);
                if (validation.Succeeded)
                {
                    total = _basketPricing.ApplyBasketDiscount(total, validation.DiscountAmount);
                }
            }

            return total;
        }
    }
}