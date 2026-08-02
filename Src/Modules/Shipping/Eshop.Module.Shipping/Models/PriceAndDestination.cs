using Shared.DDD;
using System;

namespace Eshop.Module.Shipping.Models;

public sealed class PriceAndDestination : EntityBase<Guid>
{


    public Guid CountryId { get; private set; }

    public Guid? StateOrProvinceId { get; private set; }


    public Guid? DistrictId { get; private set; }

    public string? ZipCode { get; private set; }

    public string? Note { get; private set; }

    public Money MinOrderSubtotal { get; private set; } = default!;

    public Money ShippingPrice { get; private set; } = default!;

    private PriceAndDestination() { }

    public static PriceAndDestination Create(
        Guid countryId,
        Guid? stateOrProvinceId,

        Guid? districtId,
        string? zipCode,
        string? note,
        Money minOrderSubtotal,
        Money shippingPrice)
    {
        ArgumentNullException.ThrowIfNull(minOrderSubtotal);
        ArgumentNullException.ThrowIfNull(shippingPrice);

        return new PriceAndDestination
        {
            Id = Guid.NewGuid(),
            CountryId = countryId,
            StateOrProvinceId = stateOrProvinceId,
            DistrictId = districtId,
            ZipCode = zipCode,
            Note = note,
            MinOrderSubtotal = minOrderSubtotal,
            ShippingPrice = shippingPrice
        };
    }
}
