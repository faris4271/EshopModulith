using EShop.Module.Core.Contract.Dtos;
using System.Security.Cryptography;

namespace CatalogContract.Dtos
{
    public class ProductThumbnail
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int? StockQuantity { get; set; }

        public DateTimeOffset? SpecialPriceStart { get; set; }

        public DateTimeOffset? SpecialPriceEnd { get; set; }

        public MediaDto ThumbnailImage { get; set; }

        public string ThumbnailUrl { get; set; }

        public int ReviewsCount { get; set; }

        public double? RatingAverage { get; set; }

        public CalculatedProductPrice CalculatedProductPrice { get; set; }

        public static ProductThumbnail FromProduct(Guid Id , string name,string slug, 
            decimal price, decimal? oldPrice, decimal? specialPrice,
            DateTimeOffset? specialPriceStart, DateTimeOffset? specialPriceEnd,
            int? stockQuantity, bool isAllowToOrder, bool isCallForPricing,
            MediaDto thumbnailImage)
        {
            var productThumbnail = new ProductThumbnail
            {
                Id = Id,
                Name = name,
                Slug = slug,
                Price = price,
                OldPrice = oldPrice,
                SpecialPrice = specialPrice,
                SpecialPriceStart = specialPriceStart,
                SpecialPriceEnd = specialPriceEnd,
                StockQuantity = stockQuantity,
                IsAllowToOrder = isAllowToOrder,
                IsCallForPricing = isCallForPricing,
                ThumbnailImage = thumbnailImage,
                ReviewsCount = 0,
                RatingAverage = null
            };

            return productThumbnail;
        }
    }
}