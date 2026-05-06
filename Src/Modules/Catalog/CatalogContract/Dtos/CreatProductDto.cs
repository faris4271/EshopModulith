namespace CatalogContract.Dtos
{
    public class CreatProductDto
    {


        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTimeOffset? SpecialPriceStart { get; set; }

        public DateTimeOffset? SpecialPriceEnd { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string Sku { get; set; }

        public string Gtin { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public bool IsPublished { get; set; }

        public bool IsFeatured { get; set; }

        public bool StockTrackingIsEnabled { get; set; }

        public IList<Guid> CategoryIds { get; set; } = new List<Guid>();

        public IList<ProductAttributeDto> Attributes { get; set; } = new List<ProductAttributeDto>();

        public IList<ProductOptionDto> Options { get; set; } = new List<ProductOptionDto>();

        public IList<ProductVariationDto> Variations { get; set; } = new List<ProductVariationDto>();

        public string ThumbnailImageUrl { get; set; }

        public IList<ProductMediaDto> ProductImages { get; set; } = new List<ProductMediaDto>();

        public IList<ProductMediaDto> ProductDocuments { get; set; } = new List<ProductMediaDto>();

        public IList<Guid> DeletedMediaIds { get; set; } = new List<Guid>();

        public Guid? BrandId { get; set; }

        public Guid? TaxClassId { get; set; }

        public List<ProductLinkDto> RelatedProducts { get; set; } = new List<ProductLinkDto>();

        public List<ProductLinkDto> CrossSellProducts { get; set; } = new List<ProductLinkDto>();
    }
}



