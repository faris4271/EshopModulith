using Catalog.Brands.Moddels;
using Catalog.Products.Events;
using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Products.Models
{
    public class Product : Content, IAuditableEntity
    {


        public Description ShortDescription { get; set; }

        public Description Description { get; set; }

        public string Specification { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

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
        public string Sku { get; set; }

        [StringLength(450)]
        public string Gtin { get; set; }

        [StringLength(450)]
        public string NormalizedName { get; set; }

        public int DisplayOrder { get; set; }

        public long? VendorId { get; set; }

        public Guid MainImageId { get; set; }

        public IList<ProductMedia> Medias { get; set; } = new List<ProductMedia>();

        public IList<ProductLink> ProductLinks { get; set; } = new List<ProductLink>();

        public IList<ProductLink> LinkedProductLinks { get; set; } = new List<ProductLink>();

        public IList<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();

        public IList<ProductOptionValue> OptionValues { get; set; } = new List<ProductOptionValue>();

        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

        public IList<ProductPriceHistory> PriceHistories { get; set; } = new List<ProductPriceHistory>();

        public int ReviewsCount { get; set; }

        public double? RatingAverage { get; set; }

        public Guid? BrandId { get; set; }

        public Brand Brand { get; set; }

        public Guid? TaxClassId { get; set; }

        public string CreatedById { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? LatestUpdatedOn { get; set; }

        public string? LatestUpdatedById { get; set; }



        public void AddCategory(ProductCategory category)
        {
            category.Product = this;
            Categories.Add(category);
        }

        public void AddOrUpdateFileName(Guid id)
        {
            MainImageId = id;
        }
        public void AddMedia(ProductMedia media)
        {
            media.ProductId = this.Id;
            Medias.Add(media);
        }

        public void AddAttributeValue(ProductAttributeValue attributeValue)
        {
            attributeValue.Product = this;
            AttributeValues.Add(attributeValue);
        }

        public void AddOptionValue(ProductOptionValue optionValue)
        {
            optionValue.Product = this;
            this.OptionValues.Add(optionValue);
        }


        public void AddProductLinks(ProductLink productLink)
        {
            productLink.Product = this;
            ProductLinks.Add(productLink);
        }

        public IList<ProductOptionCombination> OptionCombinations { get; set; } = new List<ProductOptionCombination>();



        public void AddOptionCombination(ProductOptionCombination combination)
        {
            combination.Product = this;
            OptionCombinations.Add(combination);
        }

        public Product Clone()
        {
            var product = new Product();
            product.Name = Name;
            product.MetaTitle = MetaTitle;
            product.MetaKeywords = MetaKeywords;
            product.MetaDescription = MetaDescription;
            product.ShortDescription = ShortDescription;
            product.Description = Description;
            product.Specification = Specification;
            product.IsPublished = IsPublished;
            product.Price = Price;
            product.OldPrice = OldPrice;
            product.SpecialPrice = SpecialPrice;
            product.SpecialPriceStart = SpecialPriceStart;
            product.SpecialPriceEnd = SpecialPriceEnd;
            product.HasOptions = HasOptions;
            product.IsVisibleIndividually = IsVisibleIndividually;
            product.IsFeatured = IsFeatured;
            product.IsAllowToOrder = IsAllowToOrder;
            product.IsCallForPricing = IsCallForPricing;
            product.StockQuantity = StockQuantity;
            product.BrandId = BrandId;
            product.VendorId = VendorId;
            product.TaxClassId = TaxClassId;
            product.StockTrackingIsEnabled = StockTrackingIsEnabled;
            product.Sku = Sku;
            product.Gtin = Gtin;
            product.NormalizedName = NormalizedName;
            product.DisplayOrder = DisplayOrder;
            product.TaxClassId = TaxClassId;
            product.Slug = Slug;

            foreach (var attribute in AttributeValues)
            {
                var att = new ProductAttributeValue(attribute.AttributeId, attribute.Value);

                product.AddAttributeValue(att);

            }

            foreach (var category in Categories)
            {
                product.AddCategory(new ProductCategory
                {
                    CategoryId = category.CategoryId
                });
            }

            return product;
        }



        public static Product Create(
            string name,
            string description,
            string shortDescription,
            decimal oldPrice,
            string specification,
            DateTimeOffset? specialPriceStart,
            DateTimeOffset? specialPriceEnd,
            string slug,
            string sku,
            string gtin,
            int stockQuantity,
            decimal? specialPrice,
            decimal price,
            string metaTitle,
            string metaDescription,
            string metaKeyword,
            bool isFeatured = false,
            bool isPublished = false,
            bool isCallForPricing = false,
            bool isAllowToOrder = true,
            bool stockTrackingIsEnabled = false
              )
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentNullException(nameof(slug));


            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(oldPrice);
            if (specialPrice.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specialPrice.Value);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stockQuantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = new Name(name),
                Slug = slug,
                NormalizedName = name.ToUpperInvariant(),
                ShortDescription = new Description(shortDescription),
                Description = new Description(description),
                Specification = specification,
                Sku = sku,
                Gtin = gtin,
                OldPrice = oldPrice,
                Price = price,
                SpecialPrice = specialPrice,
                SpecialPriceStart = specialPriceStart,
                SpecialPriceEnd = specialPriceEnd,
                IsFeatured = isFeatured,
                IsPublished = isPublished,
                IsCallForPricing = isCallForPricing,
                IsAllowToOrder = isAllowToOrder,
                StockTrackingIsEnabled = stockTrackingIsEnabled,
                StockQuantity = stockQuantity,
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeyword

            };
            product.AddDomainEvent(CreatProductEvent.Creat(product));

            return product;
        }

        public void Update(
    string name,
            string description,
            string shortDescription,
            decimal? oldPrice,
            string specification,
            DateTimeOffset? specialPriceStart,
            DateTimeOffset? specialPriceEnd,
            string slug,
            string sku,
            string gtin,
            int? stockQuantity,
            decimal? specialPrice,
            decimal? price,
            string metaTitle,
            string metaDescription,
            string metaKeyword,
            bool? isFeatured = null,
            bool? isPublished = null,
            bool? isCallForPricing = null,
            bool? isAllowToOrder = null,
            bool? stockTrackingIsEnabled = null)
        {

            if (name != null && string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (price.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price.Value);
            if (oldPrice.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(oldPrice.Value);
            if (specialPrice.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(specialPrice.Value);
            if (stockQuantity.HasValue) ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stockQuantity.Value);


            var priceChanged = false;

            if (name != null) Name = new Name(name);
            if (description != null) Description = new Shared.DDD.Description(description);
            if (shortDescription != null) ShortDescription = new Shared.DDD.Description(shortDescription);

            if (price.HasValue && Price != price.Value || SpecialPrice != specialPrice || OldPrice != oldPrice)
            {
                Price = price.Value;
                priceChanged = true;
            }

            if (oldPrice.HasValue) OldPrice = oldPrice;
            if (specialPrice.HasValue) SpecialPrice = specialPrice;
            if (specialPriceStart.HasValue) SpecialPriceStart = specialPriceStart;
            if (specialPriceEnd.HasValue) SpecialPriceEnd = specialPriceEnd;
            if (stockQuantity.HasValue) StockQuantity = stockQuantity.Value;
            if (isPublished.HasValue) IsPublished = isPublished.Value;
            if (isFeatured.HasValue) IsFeatured = isFeatured.Value;
            if (sku != null) Sku = sku;
            if (gtin != null) Gtin = gtin;


            if (priceChanged)
            {
                AddDomainEvent(ProductPriceChangeEvent.Create(this));
            }
        }
    }
}
