using Eshop.Module.Shipping.Contract.Dtos;
using Eshop.Module.Shipping.Contract.Feature.Shipping.GetApplicablePrices;
using Eshop.Module.Shipping.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.Shipping.GetApplicablePrices;

internal sealed class GetApplicableShippingPricesHandler(
    IShippingPriceService shippingPriceService)
    : IQueryHandler<GetApplicableShippingPricesQuery, List<ShippingPriceDto>>
{
    public async Task<Result<List<ShippingPriceDto>>> Handle(
        GetApplicableShippingPricesQuery query,
        CancellationToken ct)
    {
        var request = new GetShippingPriceRequest
        {
            ShippingAddress = query.ShippingAddress,
            WarehouseAddress = query.WarehouseAddress,
            ShippingItem = query.ShippingItems
                .Select(i => new ShippingItem { ProductId = i.ProductId, Quantity = i.Quantity })
                .ToList(),
            OrderAmount = query.OrderAmount
        };

        var prices = await shippingPriceService.GetApplicableShippingPrices(request);
        return Result.Success(prices.ToList());
    }
}
