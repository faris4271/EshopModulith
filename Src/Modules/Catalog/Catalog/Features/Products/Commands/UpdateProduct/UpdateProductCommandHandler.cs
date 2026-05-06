using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias;
using EShop.Module.Core.Contract.Feature.Medias.CreatMedia;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Services;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand(Guid id, ProductForm ProductForm) : ICommand<Guid>;
    internal class UpdateProductCommandHandler(IGenericeRepository<Product,
        CatalogDbContext> _repository, ICurrentUser _current, ISender sender,
        IGenericeRepository<ProductOptionValue, CatalogDbContext> _optionValueRepository)
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
                .Include(x => x.AttributeValues).ThenInclude(a => a.Attribute).ThenInclude(g => g.Group)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == request.id);

            if (product == null)
                return Result.Failure<Guid>(Error.NullValue);

            var currntUser = _current.GetUserId();

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

            //delet not complet
            var thumbNailFileCollection = new FormFileCollection();

            thumbNailFileCollection.Add(request.ProductForm.ThumbnailImage);
            var mediaId = await sender.Send(new CreatMediaCommand(thumbNailFileCollection));

            if (mediaId.IsSuccess)
                return Result.Failure<Guid>(new Error("500", "error while save media", ErrorType.Problem));
            //product.AddMedia(new ProductMedia
            //{
            //    MediaId = mediaId.Value.id,
            //    ProductId = product.Id,

            //});

            AddOrDeleteProductOption(request.ProductForm, product);
            AddOrDeleteProductAttribute(request.ProductForm, product);

            return Result.Success(product.Id);



        }

        private void AddOrDeleteProductAttribute(ProductForm productForm, Product product)
        {
            throw new NotImplementedException();
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
                        Value = JsonConvert.SerializeObject(optionValue.Value),
                        DisplayType = option.DisplayType,
                        SortIndex = optionIndex

                    });
                }

                else
                {
                    optionValue.DisplayType = option.DisplayType;
                    optionValue.Value = JsonConvert.SerializeObject(optionValue.Value);
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
    }
}
