using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Querys.GetProductByIds
{
    internal class GetProductByIdQueryHandler(
        IGenericeRepository<Product, CatalogDbContext> _productRepository,
        ISender sender) : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {

            var query = await _productRepository.Query();
            var product = query

                .Include(x => x.Medias)
                .Include(x => x.ProductLinks).ThenInclude(p => p.LinkedProduct).ThenInclude(x => x.Medias)
                .Include(x => x.OptionValues).ThenInclude(o => o.Option).Include(x => x.OptionCombinations)
                .Include(x => x.AttributeValues).ThenInclude(a => a.Attribute).ThenInclude(g => g.Group)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == request.id);



            var productVm = new ProductDto
            {
                Id = product.Id,
                Name = product.Name.name,
                Slug = product.Slug,
                MetaTitle = product.MetaTitle,
                MetaKeywords = product.MetaKeywords,
                MetaDescription = product.MetaDescription,
                Sku = product.Sku,
                Gtin = product.Gtin,
                ShortDescription = product.ShortDescription.description,
                Description = product.Description.description,
                Specification = product.Specification,
                OldPrice = product.OldPrice,
                Price = product.Price,
                SpecialPrice = product.SpecialPrice,
                SpecialPriceStart = product.SpecialPriceStart,
                SpecialPriceEnd = product.SpecialPriceEnd,
                IsFeatured = product.IsFeatured,
                IsPublished = product.IsPublished,
                IsCallForPricing = product.IsCallForPricing,
                IsAllowToOrder = product.IsAllowToOrder,
                CategoryIds = product.Categories.Select(x => x.CategoryId).ToList(),
                ThumbnailImageUrl = await GetProductImageUrl(product.MainImageId),
                BrandId = product.BrandId,
                TaxClassId = product.TaxClassId,
                StockTrackingIsEnabled = product.StockTrackingIsEnabled
            };
            var productMedias = await GetProductMedias(product.Medias.Select(x => x.MediaId).ToList());

            foreach (var productMedia in productMedias)
            {
                productVm.ProductImages.Add(new ProductMediaDto
                {
                    Id = productMedia.id,
                    MediaUrl = productMedia.FileName
                });
            }




            productVm.Options = product.OptionValues.OrderBy(x => x.SortIndex).Select(x =>
                new ProductOptionDto
                {
                    Id = x.OptionId,
                    Name = x.Option.Name,
                    DisplayType = x.DisplayType,
                    Values = JsonConvert.DeserializeObject<IList<ProductOptionValueDto>>(x.Value)
                }).ToList();

            foreach (var variation in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Super).Select(x => x.LinkedProduct).Where(x => !x.IsDeleted).OrderBy(x => x.Id))
            {
                productVm.Variations.Add(new ProductVariationDto
                {
                    Id = variation.Id,
                    Name = variation.Name.name,
                    Sku = variation.Sku,
                    Gtin = variation.Gtin,
                    Price = variation.Price,
                    OldPrice = variation.OldPrice,
                    NormalizedName = variation.NormalizedName,
                    ThumbnailImageUrl = await GetProductImageUrl(variation.MainImageId),
                    ImageUrls = (await GetProductMedias(variation.Medias.Select(m => m.MediaId).ToList())).Select(m => m.FileName).ToList(),
                    OptionCombinations = variation.OptionCombinations.Select(x => new ProductOptionCombinationDto
                    {
                        OptionId = x.OptionId,
                        OptionName = x.Option.Name,
                        Value = x.Value,
                        SortIndex = x.SortIndex
                    }).OrderBy(x => x.SortIndex).ToList()
                });
            }

            foreach (var relatedProduct in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Related).Select(x => x.LinkedProduct).Where(x => !x.IsDeleted).OrderBy(x => x.Id))
            {
                productVm.RelatedProducts.Add(new ProductLinkDto
                {
                    Id = relatedProduct.Id,
                    Name = relatedProduct.Name.name,
                    IsPublished = relatedProduct.IsPublished
                });
            }

            foreach (var crossSellProduct in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.CrossSell).Select(x => x.LinkedProduct).Where(x => !x.IsDeleted).OrderBy(x => x.Id))
            {
                productVm.CrossSellProducts.Add(new ProductLinkDto
                {
                    Id = crossSellProduct.Id,
                    Name = crossSellProduct.Name.name,
                    IsPublished = crossSellProduct.IsPublished
                });
            }

            productVm.Attributes = product.AttributeValues.Select(x => new ProductAttributeDto
            {
                Id = x.AttributeId,
                Name = x.Attribute.Name.name,
                GroupName = x.Attribute.Group.Name.name,
                Value = x.Value
            }).ToList();


            return Result.Success(productVm);
        }

        private async Task<List<MediaDto>> GetProductMedias(List<Guid> Ids)
        {
            var mediaUrls = await sender.Send(new GetListOfMediasQuery(Ids));
            return mediaUrls.Value.ToList();
        }

        private async Task<string> GetProductImageUrl(Guid id)
        {
            var mediaUrls = await sender.Send(new GetMediaByIdQuery(id));

            if (mediaUrls.IsSuccess)
            {
                return mediaUrls.Value.FirstOrDefault().FileName;
            }
            else
            {
                return null;
            }
        }
    }
}