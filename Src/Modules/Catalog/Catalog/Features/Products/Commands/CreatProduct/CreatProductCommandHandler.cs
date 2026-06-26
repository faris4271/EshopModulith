using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.CreatMedia;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Constants;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;


namespace Catalog.Features.Products.Commands.CreatProduct
{
    internal class CreatProductCommandHandler : ICommandHandler<CreatProductCommand, Guid>
    {
        private readonly IGenericeRepository<Product, CatalogDbContext> _productRepository;

        private readonly ISender _sender;

        private readonly CatalogDbContext _context;

        private readonly IGenericeRepository<ProductLink, CatalogDbContext> _productLinkRepository;
        private readonly IEntityService _entityService;





        public CreatProductCommandHandler(
            IGenericeRepository<Product, CatalogDbContext> productRepository,
            ISender sender, CatalogDbContext context,
            IGenericeRepository<ProductLink, CatalogDbContext> productLinkRepository,
            IEntityService entityService)
        {
            _productRepository = productRepository;
            _sender = sender;
            _context = context;
            _productLinkRepository = productLinkRepository;
            _entityService = entityService;
        }

        async Task<Result<Guid>> IRequestHandler<CreatProductCommand, Result<Guid>>.Handle(CreatProductCommand request, CancellationToken cancellationToken)
        {


            var productreq = request.ProductForm.Product;
            var strockQuantity = 10;

            var product = Product.Create(
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

            foreach (var option in productreq.Options)
                product.AddOptionValue(new ProductOptionValue
                {
                    DisplayType = option.DisplayType,
                    OptionId = option.Id,
                    ProductId = product.Id,
                    Value = JsonConvert.SerializeObject(option.Values)
                });

            // Inside CreatProductCommandHandler.cs
            foreach (var attribute in productreq.Attributes)
            {
                var att = new ProductAttributeValue(attribute.Id, attribute.Value);
                product.AddAttributeValue(att);
            }


            foreach (var categoryId in productreq.CategoryIds)
            {
                var productCategory = new Catalog.Products.Models.ProductCategory
                {
                    CategoryId = categoryId,
                    ProductId = product.Id,
                };
                product.AddCategory(productCategory);
            }

            if (request.ProductForm.ThumbnailImage != null)
            {
                await SaveProductMedias(request.ProductForm, product);
            }

            await MapProductVariationDtoToProduct(request.ProductForm, product);


            MapProductLinkDtoToProduct(request.ProductForm, product);


            await _productRepository.AddAsync(product);
            await _entityService.Add(product.Name.name, product.Slug, product.Id, EntityTypeConstants.Product);

            await _productRepository.SaveChangesAsync();

            return Result.Create(product.Id);
        }

        private void MapProductLinkDtoToProduct(ProductForm productForm, Product product)
        {
            foreach (var related in productForm.Product.RelatedProducts)
            {
                product.AddProductLinks(new ProductLink
                {
                    LinkType = ProductLinkType.Related,
                    Product = product,
                    LinkedProductId = related.Id

                });
            }

            foreach (var cross in productForm.Product.CrossSellProducts)
            {
                product.AddProductLinks(new ProductLink
                {
                    LinkType = ProductLinkType.CrossSell,
                    Product = product,
                    LinkedProductId = cross.Id

                });
            }

        }


        private async Task MapProductVariationDtoToProduct(ProductForm productForm, Product product)
        {

            foreach (var variant in productForm.Product.Variations)
            {
                var productLink = new ProductLink
                {
                    LinkType = ProductLinkType.Super,
                    Product = product,
                    LinkedProduct = product.Clone()
                };

                productLink.LinkedProduct.Name = new Shared.DDD.Name(productForm.Product.Name + " " + variant.Name);
                productLink.LinkedProduct.Slug = variant.Sku;
                productLink.LinkedProduct.Gtin = variant.Gtin;
                productLink.LinkedProduct.HasOptions = false;
                productLink.LinkedProduct.Price = variant.Price;
                productLink.LinkedProduct.OldPrice = variant.OldPrice;
                productLink.LinkedProduct.NormalizedName = variant.NormalizedName;
                productLink.LinkedProduct.IsVisibleIndividually = false;
                if (product.MainImageId != null)
                {
                    productLink.LinkedProduct.MainImageId = product.MainImageId;
                }


                await MapProductVariantImageFromDto(variant, productLink.LinkedProduct);

                foreach (var combination in variant.OptionCombinations)
                {
                    var option = new ProductOptionCombination
                    {
                        OptionId = combination.OptionId,
                        Value = combination.Value,
                        SortIndex = combination.SortIndex,


                    };

                    productLink.LinkedProduct.AddOptionCombination(option);
                }





                var history = CreatePriceHistory(productLink.LinkedProduct);

                product.PriceHistories.Add(history);

                product.AddProductLinks(productLink);

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

        private async Task MapProductVariantImageFromDto(ProductVariationDto productVariation, Product product)
        {
            if (productVariation.ThumbnailImage != null)
            {
                var thumbnailImage = await _sender.Send(new CreatMediaCommand(productVariation.ThumbnailImage));

                if (!thumbnailImage.IsSuccess)
                    throw new Exception(thumbnailImage.Error.ToString());



                product.AddOrUpdateFileName(thumbnailImage.Value.FirstOrDefault().id);

            }


            var filesName = await _sender.Send(new CreatMediaCommand(productVariation.NewImages));

            if (!filesName.IsSuccess)
                throw new Exception(filesName.Error.ToString());

            foreach (var file in filesName.Value)
            {
                var productMedia = new ProductMedia
                {
                    MediaId = file.id
                };
                product.AddMedia(productMedia);
            }
        }

        private async Task SaveProductMedias(ProductForm model, Product product)
        {
            var mediaFileCollection = new FormFileCollection();
            mediaFileCollection.Add(model.ThumbnailImage);
            var mediaDto = await _sender.Send(new CreatMediaCommand(mediaFileCollection));

            if (!mediaDto.IsSuccess)
                throw new ArgumentNullException(nameof(mediaDto));

            if (product.MainImageId == Guid.Empty)
            {
                product.AddOrUpdateFileName(mediaDto.Value.FirstOrDefault().id);
            }

            var productImagesCollection = new FormFileCollection();

            productImagesCollection.AddRange(model.ProductImages);

            var mediaDt = await _sender.Send(new CreatMediaCommand(productImagesCollection));

            if (!mediaDt.IsSuccess)
                throw new ArgumentNullException(nameof(mediaDt));
            foreach (var file in mediaDt.Value)
            {
                product.AddMedia(new ProductMedia
                {
                    MediaId = file.id
                });

            }

        }




    }
}
