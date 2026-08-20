using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Eshop.Module.Basket.Contract.Feature.GetProductsByIds;
using Eshop.Module.Pricing.Contract.Services;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Querys.GetProductsByIds
{
    internal class GetProductsByIdsQueryHandler(
        IGenericeRepository<Product, CatalogDbContext> _repository,
         IProductPricingService _productPricingService,
         ICurrencyService _currencyService,
         ISender sender
        )
        : IQueryHandler<GetProductsByIdsQuery, List<CartItemDto>>
    {
        public async Task<Result<List<CartItemDto>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            var products = await query.Include(x => x.OptionCombinations).
                Where(p => request.ProductIds.Contains(p.Id)).ToListAsync(cancellationToken);


            var cartItem = products.Select(x => new CartItemDto
            {

                ProductId = x.Id,
                ProductName = x.Name.name,
                ProductPrice = x.Price.Amount,
                ProductStockQuantity = x.StockQuantity,
                ProductStockTrackingIsEnabled = x.StockTrackingIsEnabled,
                IsProductAvailabeToOrder = x.IsAllowToOrder && x.IsPublished && !x.IsDeleted,
                ProductImage = GetProductImage(x.Id).Result,
                Quantity = x.StockQuantity,
                VariationOptions = x.OptionCombinations.Select(oc => new CatalogContract.Dtos.ProductVariationOptionDto
                {
                    OptionName = oc.Option.Name,
                    Value = oc.Value
                }).ToList()

            }).ToList();

            return Result.Success(cartItem);

        }

        private async Task<string> GetProductImage(Guid id)
        {
            var value = await sender.Send(new GetMediaByIdQuery(id));

            return value.IsSuccess ? value.Value.FirstOrDefault()?.FileName : string.Empty;
        }
    }
}
