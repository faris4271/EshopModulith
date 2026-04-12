using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using EShop.Module.Core.Contract.Feature.Medias;
using MediatR;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Catalog.Features.Products.Commands.CreatProduct.CreatProductCommandHandler;

namespace Catalog.Features.Products.Commands.CreatProduct
{
    internal class CreatProductCommandHandler : ICommandHandler<CreatProductCommand, Guid>
    {
        private readonly IGenericeRepository<Product, CatalogDbContext> _productRepository;

        private readonly ISender _sender;


        public record CreatProductCommand(ProductForm ProductForm) : ICommand<Guid>;


        public CreatProductCommandHandler(IGenericeRepository<Product, CatalogDbContext> productRepository, ISender sender)
        {
            _productRepository = productRepository;
            _sender = sender;
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

            foreach (var attribute in productreq.Attributes)
            {
                var att = new ProductAttributeValue
                {
                    AttributeId = attribute.Id,
                    Value = attribute.Value,
                };
                product.AddAttributeValue(att);
            }


            foreach (var categoryId in productreq.CategoryIds)
            {
                var productCategory = new Catalog.Products.Models.ProductCategory
                {
                    CategoryId = categoryId,
                };
                product.AddCategory(productCategory);
            }

            await SaveProductMedias(request.ProductForm, product);

            await MapProductVariationDtoToProduct(request.ProductForm, product);
            

            MapProductLinkDtoToProduct(request.ProductForm, product);


            _productRepository.Add(product);

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

                productLink.LinkedProduct.Name = new Shared.DDD.Name(variant.Name);
                productLink.LinkedProduct.Slug = variant.Sku;
                productLink.LinkedProduct.Gtin = variant.Gtin;
                productLink.LinkedProduct.HasOptions = false;
                productLink.LinkedProduct.Price = variant.Price;
                productLink.LinkedProduct.OldPrice = variant.OldPrice;
                productLink.LinkedProduct.NormalizedName = variant.NormalizedName;
                productLink.LinkedProduct.IsVisibleIndividually = false;

                await MapProductVariantImageFromDto(variant, product);

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
            if (productVariation.ThumbnailImage == null)
            {
                var thumbnailImage = await _sender.Send(new CreatMediaCommand(productVariation.ThumbnailImage));

                if (!thumbnailImage.IsSuccess)
                    throw new Exception(thumbnailImage.Error.ToString());



                product.AddOrUpdateFileName(thumbnailImage.Value.FileName);

            }

            foreach (var file in productVariation.NewImages)
            {
                var fileName = await _sender.Send(new CreatMediaCommand(file));

                if (!fileName.IsSuccess)
                    throw new Exception(fileName.Error.ToString());

                var productMedia = new ProductMedia
                {
                    MediaId = fileName.Value.id,

                };
                product.AddMedia(productMedia);
            }
        }
        private async Task SaveProductMedias(ProductForm model, Product product)
        {

            var mediaDto = await _sender.Send(new CreatMediaCommand(model.ThumbnailImage));

            if (!mediaDto.IsSuccess)
                throw new ArgumentNullException(nameof(mediaDto));

            if (product.ThumbnailImage != null)
            {
                product.ThumbnailImage = mediaDto.Value.FileName;
            }


            foreach (var file in model.ProductImages)
            {
                var mediaDt = await _sender.Send(new CreatMediaCommand(file));

                if (!mediaDt.IsSuccess)
                    throw new ArgumentNullException(nameof(mediaDt));

                product.AddMedia(new ProductMedia
                {
                    MediaId = mediaDt.Value.id
                });

            }

        }




    }
}
