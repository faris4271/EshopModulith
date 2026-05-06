using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using CatalogContract.Services;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Products.Querys.GetProductDetails
{
    internal class GetProductDetailsQueryHandler(
        IGenericeRepository<Product, CatalogDbContext> _repository,
        IProductPricingService _productPricingService,
        ISender _sender
        ) : IQueryHandler<GetProductDetailsQuery, ProductDetail>
    {

        public async Task<Result<ProductDetail>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var query =await _repository.GetAllAsQuerable();

            var product =await query.FirstOrDefaultAsync(p => p.Id == request.Id);

            var model = new ProductDetail
            {
                Id = product.Id,
                Name = product.Name.name,
                ShortDescription = product.ShortDescription.description,
                MetaTitle = product.MetaTitle,
                MetaKeywords = product.MetaKeywords,
                MetaDescription = product.MetaDescription,
                Description = product.Description.description,
                Specification = product.Specification,
                IsCallForPricing = product.IsCallForPricing,
                IsAllowToOrder = product.IsAllowToOrder,
                StockTrackingIsEnabled = product.StockTrackingIsEnabled,
                StockQuantity = product.StockQuantity,
                Categories= product.Categories.Select(c => new ProductDetailCategory
                {
                    Id = c.CategoryId,
                    Name = c.Category.Name.name,
                    Slug = c.Category.Slug
                }).ToList(),

                CalculatedProductPrice = _productPricingService.CalculateProductPrice(product.Price, product.OldPrice, product.SpecialPrice, product.SpecialPriceStart, product.SpecialPriceEnd),
                Attributes = product.AttributeValues.Select(a => new ProductDetailAttribute
                {
                    Name = a.Attribute.Name.name,
                    Value = a.Value
                }).ToList(),

                 };

            await MapProductVariantToProductDetailsVariant(product, model);
            await MapRelatedProductToProductDetails(product, model);
            MapProductOptionToProductDetails(product, model);
            await MapProductImagesToProductVm(product, model);


            return Result.Success(model);



        }

        private async Task MapProductImagesToProductVm(Product product, ProductDetail model)
        {

            var medias =await _sender.Send(new GetListOfMediasQuery(product.Medias.Select(m => m.Id).ToList()));

           

            model.Images = medias.IsSuccess ? medias.Value.Select(m => new MediaDto
            (
              m.id,
              m.FileName
            )).ToList() : new List<MediaDto>();
        }

        private async Task MapProductVariantToProductDetailsVariant(Product product, ProductDetail model)
        {
            if(!product.ProductLinks.Any(x=>x.LinkType==ProductLinkType.Super))
                return;

            var query =await _repository.Query();

            var variants =await query.Include(x => x.OptionCombinations).ThenInclude(x => x.Option).
                Include(x => x.Medias).Where(x=>x.ProductLinks.
                Any(link=>link.LinkType==ProductLinkType.Super && link.ProductId==product.Id)).
                Where(x=>x.IsPublished).ToListAsync();

            foreach (var variation in variants)
            {
                var medias = await _sender.Send(new GetMediaByIdQuery (variation.Id));

                var variantDetails = new ProductDetailVariation
                {
                    Id = variation.Id,
                    Name = variation.Name.name,
                    NormalizedName = variation.NormalizedName,
                    IsAllowToOrder = variation.IsAllowToOrder,
                    IsCallForPricing = variation.IsCallForPricing,
                    StockTrackingIsEnabled = variation.StockTrackingIsEnabled,
                    StockQuantity = variation.StockQuantity,
                    CalculatedProductPrice = _productPricingService.
                    CalculateProductPrice(
                        variation.Price, variation.OldPrice, variation.SpecialPrice,
                        variation.SpecialPriceStart, variation.SpecialPriceEnd),
                    Images =medias.IsSuccess ? medias.Value.Select(m => new MediaDto
                    (
                      m.id,
                      m.FileName
                    )).ToList() : new List<MediaDto>(),
                };

                foreach (var optionCombination in variation.OptionCombinations)
                {
                    variantDetails.Options.Add(new ProductDetailVariationOption
                    {
                       OptionId= optionCombination.OptionId,
                       OptionName= optionCombination.Option.Name,
                       Value= optionCombination.Value,

                    });

                }
                model.Variations.Add(variantDetails);
            }

           


        }

        private async Task MapRelatedProductToProductDetails(Product product, ProductDetail model)
        {
            var publishedProductLinks = product.ProductLinks.Where(
                x=>x.LinkedProduct.IsPublished && 
                (x.LinkType == ProductLinkType.Related 
                || x.LinkType == ProductLinkType.CrossSell)).ToList();

            foreach (var productLink in publishedProductLinks)
            {
                var linkedProduct = productLink.LinkedProduct;

                var productThumbnail =ProductThumbnail.FromProduct(
                    linkedProduct.Id, linkedProduct.Name.name, linkedProduct.Slug,
                    linkedProduct.Price, linkedProduct.OldPrice, linkedProduct.SpecialPrice,
                    linkedProduct.SpecialPriceStart, linkedProduct.SpecialPriceEnd,
                    linkedProduct.StockQuantity,linkedProduct.IsAllowToOrder, linkedProduct.IsCallForPricing, null);
                 var mediaResult = await _sender.Send(new GetMediaByIdQuery(linkedProduct.Id));
                productThumbnail.ThumbnailUrl = mediaResult.IsSuccess ? mediaResult.Value.FirstOrDefault()?.FileName : null;

                productThumbnail.CalculatedProductPrice = _productPricingService.CalculateProductPrice(
                    linkedProduct.Price, linkedProduct.OldPrice, linkedProduct.SpecialPrice,
                    linkedProduct.SpecialPriceStart, linkedProduct.SpecialPriceEnd);

                if(productLink.LinkType == ProductLinkType.Related)
                    model.RelatedProducts.Add(productThumbnail);

                else if(productLink.LinkType == ProductLinkType.CrossSell)
                    model.CrossSellProducts.Add(productThumbnail);
            }

           
        }
        private void MapProductOptionToProductDetails(Product product, ProductDetail model)
        {
            foreach (var item in product.OptionValues)
            {
                var optionValues = JsonConvert.DeserializeObject<IList<ProductOptionValueDto>>(item.Value);
                foreach (var value in optionValues)
                {
                    if (!model.OptionDisplayValues.ContainsKey(value.Key))
                    {
                        model.OptionDisplayValues.Add(value.Key, new ProductOptionDisplay { DisplayType = item.DisplayType, Value = value.Display });
                    }
                }
            }
        }

    }
}
