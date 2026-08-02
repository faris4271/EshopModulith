namespace CatalogContract.Dtos;

using EShop.Module.Core.Contract.Dtos;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CartItemsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Specification { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public Money Price { get; set; } = Money.Zero();
    public Money? OldPrice { get; set; }
    public Money? SpecialPrice { get; set; }

    public DateTimeOffset? SpecialPriceStart { get; set; }
    public DateTimeOffset? SpecialPriceEnd { get; set; }
    public bool HasOptions { get; set; }
    public bool IsVisibleIndividually { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsCallForPricing { get; set; }
    public bool IsAllowToOrder { get; set; }
    public bool StockTrackingIsEnabled { get; set; }
    public int StockQuantity { get; set; }

    [StringLength(450)]
    public string Sku { get; set; } = string.Empty;

    [StringLength(450)]
    public string Gtin { get; set; } = string.Empty;

    [StringLength(450)]
    public string NormalizedName { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
    public long? VendorId { get; set; }
    public long? BrandId { get; set; }
    public long? TaxClassId { get; set; }

    public MediaDto? ThumbnailMedia { get; set; }
    public List<string> ThumbnailUrls { get; set; } = new();

    public int ReviewsCount { get; set; }
    public double? RatingAverage { get; set; }
}