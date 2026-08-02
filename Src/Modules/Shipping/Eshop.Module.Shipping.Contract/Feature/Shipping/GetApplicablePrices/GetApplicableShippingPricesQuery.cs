using Eshop.Module.Shipping.Contract.Dtos;
using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Shipping.Contract.Feature.Shipping.GetApplicablePrices;

public sealed record GetApplicableShippingPricesQuery(
    ShippingAddressDto ShippingAddress,
    ShippingAddressDto WarehouseAddress,
    IList<ShippingItemDto> ShippingItems,
    decimal OrderAmount) : IQuery<List<ShippingPriceDto>>;
