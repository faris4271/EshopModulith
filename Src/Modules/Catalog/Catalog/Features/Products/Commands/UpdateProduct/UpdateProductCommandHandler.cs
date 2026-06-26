using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.CreatMedia;
using EShop.Module.Core.Contract.Feature.Medias.UpdateMedia;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Constants;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.UpdateProduct
{
    //that need to handle photo update and delete and add new photo and option and attribute
    internal class UpdateProductCommandHandler(IGenericeRepository<Product,
        CatalogDbContext> _repository, ISender sender,
        IGenericeRepository<ProductOptionValue, CatalogDbContext> _optionValueRepository,
        IGenericeRepository<ProductLink, CatalogDbContext> _productLinkRepository,
        IGenericeRepository<ProductAttributeValue, CatalogDbContext> _productAttributeValueRepository,
        IEntityService _entityService)
        : ICommandHandler<UpdateProductCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var query = await _repository.GetAllAsQuerable();

            var product = query.Include(x => x.ProductLinks).
                ThenInclude(x => x.LinkedProduct).
                ThenInclude(p => p.Medias)
               .Include(x => x.OptionValues).
                ThenInclude(o => o.Option)
                .Include(x => x.AttributeValues).ThenInclude(a => a.Attribute).ThenInclude(g => g.Group).AsTracking()
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == request.id);

            if (product == null)
                return Result.Failure<Guid>(Error.NullValue);

            var productreq = request.ProductForm.Product;
            var strockQuantity = 10;

            product.Update(
               productreq.Name,
               productreq.Description,
               productreq.ShortDescription,
               productreq.OldPrice ?? 0m,
               productreq.Specification,
               productreq.SpecialPriceStart,
               productreq.SpecialPriceEnd,
               productreq.Slug,
               productreq.Sku,
               productreq.Gtin,
               strockQuantity,
               productreq.SpecialPrice,
               productreq.Price,
               productreq.MetaTitle,
               productreq.MetaDescription,
               productreq.MetaKeywords);

            var emagesDelete = product.Medias.Where(x => request.ProductForm.Product.DeletedMediaIds.Contains(x.MediaId)).ToList();
            if (request.ProductForm.ThumbnailImage != null && request.ProductForm.ThumbnailImage.Length > 0)
            {
                var thumbnailImage = new FormFileCollection();
                thumbnailImage.Add(request.ProductForm.ThumbnailImage);
                var updateMdia = await sender.Send(new UpdateMediaCommand(product.Medias.Select(x => x.MediaId).ToList(), thumbnailImage));

                if (!updateMdia.IsSuccess)
                    return Result.Failure<Guid>(new Error("500", "error while save media", ErrorType.Problem));
                product.AddMedia(new ProductMedia
                {
                    MediaId = updateMdia.Value[0].id,
                    ProductId = product.Id,

                });
            }


            AddOrDeleteProductOption(request.ProductForm, product);
            AddOrDeleteProductAttribute(request.ProductForm, product);
            AddOrDeleteCategories(request.ProductForm, product);
            await AddOrDeleteProductVariation(request.ProductForm, product);
            await AddOrDeleteProductLinks(request.ProductForm, product);
            await _entityService.Update(product.Name.name, product.Slug, product.Id, EntityTypeConstants.Product);
            await _repository.SaveChangesAsync();

            return Result.Success(product.Id);



        }

        private void AddOrDeleteProductAttribute(ProductForm model, Product product)
        {
            foreach (var productAttributeVm in model.Product.Attributes)
            {
                var productAttrValue =
                    product.AttributeValues.FirstOrDefault(x => x.AttributeId == productAttributeVm.Id);
                if (productAttrValue == null)
                {
                    productAttrValue = new ProductAttributeValue(productAttributeVm.Id, productAttributeVm.Value);

                    product.AddAttributeValue(productAttrValue);
                }
                else
                {
                    productAttrValue.Value = productAttributeVm.Value;
                }
            }

            var deletedAttrValues =
                product.AttributeValues.Where(attrValue => model.Product.Attributes.All(x => x.Id != attrValue.AttributeId))
                    .ToList();

            foreach (var deletedAttrValue in deletedAttrValues)
            {
                deletedAttrValue.Product = null;
                product.AttributeValues.Remove(deletedAttrValue);
                _productAttributeValueRepository.Delete(deletedAttrValue);
            }
        }

        private void AddOrDeleteProductOption(ProductForm model, Product product)
        {
            var optionIndex = 0;

            foreach (var option in model.Product.Options)
            {
                var optionValue = product.OptionValues.FirstOrDefault(x => x.OptionId == option.Id);

                if (optionValue == null)
                {
                    product.AddOptionValue(new ProductOptionValue
                    {
                        OptionId = option.Id,
                        Value = JsonConvert.SerializeObject(option.Values),
                        DisplayType = option.DisplayType,
                        SortIndex = optionIndex

                    });
                }

                else
                {
                    optionValue.DisplayType = option.DisplayType;
                    optionValue.Value = JsonConvert.SerializeObject(option.Values);
                    optionValue.SortIndex = optionIndex;
                }

                optionIndex++;
            }

            var deletedProductOptionValues = product.OptionValues
                .Where(x => model.Product.Options.All(vm => vm.Id != x.OptionId)).ToList();

            foreach (var deletedOption in deletedProductOptionValues)
            {
                product.OptionValues.Remove(deletedOption);
                _optionValueRepository.Delete(deletedOption);

            }
        }

        private void AddOrDeleteCategories(ProductForm model, Product product)
        {
            foreach (var categoryId in model.Product.CategoryIds)
            {
                if (model.Product.CategoryIds.Any(id => id == categoryId))
                {
                    continue;
                }

                product.AddCategory(new ProductCategory { CategoryId = categoryId });

            }


            var deletedProductCategories =
                product.Categories.Where(productCategory => !model.Product.CategoryIds.Contains(productCategory.CategoryId))
                    .ToList();

            foreach (var deletedProductCategory in deletedProductCategories)
            {
                deletedProductCategory.Product = null;
                product.Categories.Remove(deletedProductCategory);
                _repository.Delete(deletedProductCategory);
            }

        }
        private async Task AddOrDeleteProductVariation(ProductForm model, Product product)
        {
            foreach (var productVariationVm in model.Product.Variations)
            {
                var productLink = product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Super).FirstOrDefault(x => x.LinkedProduct.Name.name == productVariationVm.Name);
                if (productLink == null)
                {
                    productLink = new ProductLink
                    {
                        LinkType = ProductLinkType.Super,
                        Product = product,
                        LinkedProduct = product.Clone()
                    };

                    productLink.LinkedProduct.Name = new Shared.DDD.Name(productVariationVm.Name);
                    productLink.LinkedProduct.Slug = productVariationVm.Name;
                    productLink.LinkedProduct.Sku = productVariationVm.Sku;
                    productLink.LinkedProduct.Gtin = productVariationVm.Gtin;
                    productLink.LinkedProduct.Price = productVariationVm.Price;
                    productLink.LinkedProduct.OldPrice = productVariationVm.OldPrice;
                    productLink.LinkedProduct.NormalizedName = productVariationVm.NormalizedName;
                    productLink.LinkedProduct.HasOptions = false;
                    productLink.LinkedProduct.IsVisibleIndividually = false;
                    if (product.MainImageId != null)
                    {
                        productLink.LinkedProduct.MainImageId = product.MainImageId;
                    }

                    await MapProductVariantImageFromDto(productVariationVm, productLink.LinkedProduct);

                    foreach (var combinationVm in productVariationVm.OptionCombinations)
                    {
                        productLink.LinkedProduct.AddOptionCombination(new ProductOptionCombination
                        {
                            OptionId = combinationVm.OptionId,
                            Value = combinationVm.Value,
                            SortIndex = combinationVm.SortIndex
                        });
                    }

                    var productPriceHistory = CreatePriceHistory(productLink.LinkedProduct);
                    productLink.LinkedProduct.PriceHistories.Add(productPriceHistory);

                    product.AddProductLinks(productLink);
                }
                else
                {
                    var isPriceChanged = false;
                    if (productLink.LinkedProduct.Price != productVariationVm.Price ||
                        productLink.LinkedProduct.OldPrice != productVariationVm.OldPrice)
                    {
                        isPriceChanged = true;
                    }


                    productLink.LinkedProduct.LatestUpdatedOn = DateTimeOffset.Now;
                    productLink.LinkedProduct.Sku = productVariationVm.Sku;
                    productLink.LinkedProduct.Gtin = productVariationVm.Gtin;
                    productLink.LinkedProduct.Price = productVariationVm.Price;
                    productLink.LinkedProduct.OldPrice = productVariationVm.OldPrice;
                    productLink.LinkedProduct.IsDeleted = false;
                    productLink.LinkedProduct.StockTrackingIsEnabled = product.StockTrackingIsEnabled;

                    await MapProductVariantImageFromDto(productVariationVm, productLink.LinkedProduct);

                    if (isPriceChanged)
                    {
                        var productPriceHistory = CreatePriceHistory(productLink.LinkedProduct);
                        productLink.LinkedProduct.PriceHistories.Add(productPriceHistory);
                    }

                }

            }

            foreach (var productLink in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Super))
            {
                if (model.Product.Variations.All(x => x.Name != productLink.LinkedProduct.Name.name))
                {
                    _productLinkRepository.Delete(productLink);
                    productLink.LinkedProduct.IsDeleted = true;
                }
            }
        }
        private async Task AddOrDeleteProductLinks(ProductForm model, Product product)
        {
            foreach (var relatedProductVm in model.Product.RelatedProducts)
            {
                var productLink = product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Related).FirstOrDefault(x => x.LinkedProductId == relatedProductVm.Id);
                if (productLink == null)
                {
                    productLink = new ProductLink
                    {
                        LinkType = ProductLinkType.Related,
                        Product = product,
                        LinkedProductId = relatedProductVm.Id,
                    };

                    await _productLinkRepository.AddAsync(productLink);
                }
            }

            foreach (var productLink in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.Related))
            {
                if (model.Product.RelatedProducts.All(x => x.Id != productLink.LinkedProductId))
                {
                    _productLinkRepository.Delete(productLink);
                }
            }

            foreach (var crossSellProductVm in model.Product.CrossSellProducts)
            {
                var productLink = product.ProductLinks.Where(x => x.LinkType == ProductLinkType.CrossSell).FirstOrDefault(x => x.LinkedProductId == crossSellProductVm.Id);
                if (productLink == null)
                {
                    productLink = new ProductLink
                    {
                        LinkType = ProductLinkType.CrossSell,
                        Product = product,
                        LinkedProductId = crossSellProductVm.Id,
                    };

                    _productLinkRepository.Add(productLink);
                }
            }

            foreach (var productLink in product.ProductLinks.Where(x => x.LinkType == ProductLinkType.CrossSell))
            {
                if (model.Product.CrossSellProducts.All(x => x.Id != productLink.LinkedProductId))
                {
                    _productLinkRepository.Delete(productLink);
                }
            }
        }


        private async Task MapProductVariantImageFromDto(ProductVariationDto productVariation, Product product)
        {
            if (productVariation.ThumbnailImage != null && productVariation.ThumbnailImage.Any(file => file != null && file.Length > 0))
            {
                var thumbnailImage = await sender.Send(new CreatMediaCommand(productVariation.ThumbnailImage));

                if (!thumbnailImage.IsSuccess)
                    throw new Exception(thumbnailImage.Error.ToString());

                product.AddOrUpdateFileName(thumbnailImage.Value.FirstOrDefault().id);

            }

            if (productVariation.NewImages != null && productVariation.NewImages.Any(file => file != null && file.Length > 0))
            {

                var filesName = await sender.Send(new CreatMediaCommand(productVariation.NewImages));

                if (!filesName.IsSuccess)
                    throw new Exception(filesName.Error.ToString());

                var productMedia = new ProductMedia
                {
                    MediaId = filesName.Value.FirstOrDefault().id,
                };
                product.AddMedia(productMedia);
            }
        }
        private static ProductPriceHistory CreatePriceHistory(Product product)
        {
            return new ProductPriceHistory
            {

                Product = product,
                Price = product.Price,
                OldPrice = product.OldPrice,
                SpecialPrice = product.SpecialPrice,
                SpecialPriceStart = product.SpecialPriceStart,
                SpecialPriceEnd = product.SpecialPriceEnd
            };
        }
    }
}