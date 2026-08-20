namespace Eshop.Module.Checkout.Services;

using CatalogContract.Dtos;
using CatalogContract.Features.GetCheckoutItemWithTax;
using CatalogContract.Features.GetCheckOutProducts;
using Eshop.Module.Checkout.Contract.Dtos;
using Eshop.Module.Checkout.Contract.Services;
using Eshop.Module.Checkout.Data;
using Eshop.Module.Pricing.Contract.Services;
using Eshop.Module.Shipping.Contract.Dtos;
using Eshop.Module.Shipping.Contract.Feature.Shipping.GetApplicablePrices;
using EShop.Module.Core.Contract.Dtos;
using global::Module.Identity.Contract.Feature.Users.GetUserAddrees;
using global::Module.Tax.Contract.Features.GetTaxPercents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Abstraction;
using Shared.Contract.Exeption;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

internal class CheckoutService : ICheckoutService
{
    private readonly IGenericeRepository<Entities.Checkout, CheckoutDbContext> _checkoutRepository;
    private readonly ICurrencyService _currencyService;
    private readonly IGenericeRepository<Entities.CheckoutItem, CheckoutDbContext> _checkoutItemRepository;
    private readonly ISender _sender;
    private readonly ICouponService _couponService;
    private readonly IConfiguration _configuration;
    private readonly bool _isProductPriceIncludeTax;

    public CheckoutService(
        IGenericeRepository<Entities.Checkout, CheckoutDbContext> checkoutRepository,
        ICurrencyService currencyService,
        IGenericeRepository<Entities.CheckoutItem, CheckoutDbContext> checkoutItemRepository,
        ISender sender,
        ICouponService couponService,
        IConfiguration configuration
    )
    {
        _checkoutRepository = checkoutRepository ?? throw new ArgumentNullException(nameof(checkoutRepository));
        _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        _checkoutItemRepository = checkoutItemRepository ?? throw new ArgumentNullException(nameof(checkoutItemRepository));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _isProductPriceIncludeTax = configuration.GetValue<bool>("IsProductPriceIncludeTax");
    }

    public async Task<CreatCheckoutResultDto> Create(Guid customerId, Guid createdById, IList<CartItemToCheckoutDto> cartItems, string couponCode)
    {
        var checkout = Entities.Checkout.Create(customerId, _isProductPriceIncludeTax, couponCode);

        foreach (var item in cartItems)
        {
            checkout.AddItem(item.ProductId, item.Quantity);
        }

        await _checkoutRepository.AddAsync(checkout);
        await _checkoutRepository.SaveChangesAsync();

        var result = new CreatCheckoutResultDto(checkout.Id, customerId, _isProductPriceIncludeTax, couponCode, string.Empty, DateTimeOffset.UtcNow);




        return result;
    }

    public async Task<CheckoutDto> GetCheckoutDetails(Guid checkoutId)
    {
        var checkoutQuery = await _checkoutRepository.Query();
        var checkoutEntity = await checkoutQuery.FirstOrDefaultAsync(c => c.Id == checkoutId);

        if (checkoutEntity is null)
        {
            throw new CustomException($"Checkout with ID '{checkoutId}' was not found.");
        }
        var checkoutItemsQuery = await _checkoutItemRepository.Query();
        var checkoutItems = await checkoutItemsQuery
            .Where(x => x.CheckoutId == checkoutId)
            .ToListAsync();

        var productIds = checkoutItems.Select(x => x.ProductId).Distinct().ToList();

        // 2. جلب المنتجات عبر MediatR
        var checkoutProductItems = await _sender.Send(new GetCheckOutProductsQuery(productIds));
        if (!checkoutProductItems.IsSuccess)
        {
            throw new CustomException(checkoutProductItems.Error.Description);
        }

        var itemsList = checkoutProductItems.Value ?? new List<CheckoutItemDto>();
        var checkoutItemQuantities = checkoutItems.ToDictionary(x => x.ProductId, x => x.Quantity);

        foreach (var item in itemsList)
        {
            if (checkoutItemQuantities.TryGetValue(item.ProductId, out var quantity))
            {
                item.Quantity = quantity;
            }
        }

        var subTotalAmount = itemsList.Sum(x => x.Quantity * (x.CalculatedProductPrice?.OldPrice ?? x.ProductPrice));

        var checkoutDto = new CheckoutDto
        {
            Id = checkoutEntity.Id,
            CouponCode = checkoutEntity.CouponCode,
            IsProductPriceIncludeTax = checkoutEntity.IsProductPriceIncludeTax,
            TaxAmount = checkoutEntity.TaxAmount?.Amount,
            ShippingAmount = checkoutEntity.ShippingAmount?.Amount,
            OrderNote = checkoutEntity.OrderNote,
            SubTotal = subTotalAmount,
            Items = itemsList,
            OrderTotalString = string.Empty,
            TaxAmountString = _currencyService.FormatCurrency(checkoutEntity.TaxAmount ?? new Money(0m, "USD")),
            ShippingAmountString = _currencyService.FormatCurrency(checkoutEntity.ShippingAmount ?? new Money(0m, "USD"))
        };

        checkoutDto.OrderTotalString = _currencyService.FormatCurrency(new Money(checkoutDto.OrderTotal, "USD"));

        if (!string.IsNullOrWhiteSpace(checkoutDto.CouponCode))
        {
            var cartInfoForCoupon = new Eshop.Module.Pricing.Contract.Dtos.CartInfoForCoupon
            {
                Items = checkoutDto.Items.Select(x => new Eshop.Module.Pricing.Contract.Dtos.CartItemForCoupon
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var couponValidationResult = await _couponService.Validate(checkoutEntity.UserId, checkoutEntity.CouponCode, cartInfoForCoupon);

            if (couponValidationResult.Succeeded)
            {
                checkoutDto.Discount = couponValidationResult.DiscountAmount;
            }
            else
            {
                checkoutDto.CouponValidationErrorMessage = couponValidationResult.ErrorMessage;
            }
        }

        return checkoutDto;
    }

    public async Task<CheckoutTaxAndShippingPriceDto> UpdateTaxAndShippingPrices(Guid checkoutId, TaxAndShippingPriceRequestDto model)
    {
        var checkoutQuery = await _checkoutRepository.Query();
        var checkoutEntity = await checkoutQuery.FirstOrDefaultAsync(c => c.Id == checkoutId);

        if (checkoutEntity is null)
        {
            throw new CustomException("Checkout not found");
        }

        ShippingAddressDto shippingAddress;

        if (model.ExistingShippingAddressId != Guid.Empty)
        {
            var addressResult = await _sender.Send(new GetUserAddreesQuery(model.ExistingShippingAddressId));
            if (!addressResult.IsSuccess)
            {
                throw new CustomException(addressResult.Error.Description);
            }

            var userAddress = addressResult.Value;
            shippingAddress = userAddress.Addresses.FirstOrDefault()
                ?? throw new CustomException("Selected shipping address was not found.");
        }
        else if (model.NewShippingAddress is not null)
        {
            shippingAddress = new ShippingAddressDto
            {
                CountryId = model.NewShippingAddress.CountryId,
                StateOrProvinceId = model.NewShippingAddress.StateOrProvinceId,
                DistrictId = model.NewShippingAddress.DistrictId,
                ZipCode = model.NewShippingAddress.ZipCode,
                AddressLine1 = model.NewShippingAddress.AddressLine1,
            };
        }
        else
        {
            throw new CustomException("Shipping address information is missing.");
        }


        var orderTaxAndShippingPrice = new CheckoutTaxAndShippingPriceDto
        {
            CheckoutDto = await GetCheckoutDetails(checkoutId)
        };


        var calculatedTax = await GetTax(
            checkoutId,
            shippingAddress.CountryId,
            shippingAddress.StateOrProvinceId,
            shippingAddress.ZipCode
        );

        var applicableShippingPricesResult = await _sender.Send(new GetApplicableShippingPricesQuery(
            shippingAddress,
            shippingAddress,
            orderTaxAndShippingPrice.CheckoutDto.Items
                .Select(x => new ShippingItemDto(x.ProductId, x.Quantity))
                .ToList(),
            orderTaxAndShippingPrice.CheckoutDto.OrderTotal
        ));

        if (!applicableShippingPricesResult.IsSuccess)
        {
            throw new CustomException(applicableShippingPricesResult.Error.Description);
        }

        orderTaxAndShippingPrice.ShippingPrices = applicableShippingPricesResult.Value;

        var selectedShippingMethod = string.IsNullOrWhiteSpace(model.SelectedShippingMethodName)
            ? orderTaxAndShippingPrice.ShippingPrices.FirstOrDefault()
            : orderTaxAndShippingPrice.ShippingPrices.FirstOrDefault(x => x.Name == model.SelectedShippingMethodName);


        if (selectedShippingMethod is not null)
        {
            var shippingMoney = new Money(selectedShippingMethod.Price, selectedShippingMethod.Currency ?? "USD");
            var taxMoney = new Money(calculatedTax, shippingMoney.Currency);

            checkoutEntity.AddAmuntAndShippingMethud(selectedShippingMethod.Name, shippingMoney);
            checkoutEntity.UpdateTaxAmount(taxMoney);

            await _checkoutRepository.SaveChangesAsync();
        }

        return orderTaxAndShippingPrice;
    }

    public async Task<decimal> GetTax(Guid checkoutId, Guid countryId, Guid stateOrProvinceId, string zipCode)
    {
        var checkoutItemsQuery = await _checkoutItemRepository.Query();

        var checkoutItems = await checkoutItemsQuery
            .Where(x => x.CheckoutId == checkoutId)
            .ToListAsync();

        if (!checkoutItems.Any())
        {
            return 0m;
        }

        var itemQuantities = checkoutItems.ToDictionary(x => x.ProductId, x => x.Quantity);
        var productIds = checkoutItems.Select(x => x.ProductId).Distinct().ToList();

        var checkoutItemsResult = await _sender.Send(new GetCheckoutItemWithTaxQuery(productIds));
        if (!checkoutItemsResult.IsSuccess)
        {
            throw new CustomException(checkoutItemsResult.Error.Description);
        }

        var taxAmount = 0m;
        var checkoutItemsDto = checkoutItemsResult.Value;


        foreach (var item in checkoutItemsDto)
        {
            if (item.taxClassId.HasValue)
            {
                var quantity = itemQuantities.TryGetValue(item.productId, out var qty) ? qty : 0;

                var taxRateResult = await _sender.Send(new GetTaxPercentQuery(item.taxClassId, countryId, stateOrProvinceId, zipCode));
                if (!taxRateResult.IsSuccess)
                {
                    throw new CustomException(taxRateResult.Error.Description);
                }

                var taxRate = taxRateResult.Value;
                taxAmount += quantity * item.productPrice * (taxRate / 100m);
            }
        }

        return taxAmount;
    }
}