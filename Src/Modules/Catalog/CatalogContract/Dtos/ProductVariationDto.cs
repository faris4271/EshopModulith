using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CatalogContract.Dtos
{
    public class ProductVariationDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Sku { get; set; }

        public string Gtin { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        // File input doesn't get bound on nested models
        // https://github.com/aspnet/Mvc/issues/4485
        // Workaround by moving file input to the top
        [BindNever]
        public IFormFileCollection ThumbnailImage { get; set; }

        public string ThumbnailImageUrl { get; set; }

        [BindNever]
        public IFormFileCollection NewImages { get; set; }

        public IList<string> ImageUrls { get; set; } = new List<string>();

        public IList<ProductOptionCombinationDto> OptionCombinations { get; set; } =
            new List<ProductOptionCombinationDto>();
    }
}