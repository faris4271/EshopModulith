using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Services;
using Eshop.Module.Basket.Contract.Dtos;
using Eshop.Module.Basket.Contract.Feature.GetProductsByIds;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.Products.Querys.GetProductsByIds
{
    internal class GetProductsByIdsQueryHandler(
        IGenericeRepository<Product,CatalogDbContext> _repository,
         IProductPricingService _productPricingService,
         ICurrencyService _currencyService,
         ISender sender
        )
        : IQueryHandler<GetProductsByIdsQuery, List<CartItemDto>>
    {
        public async Task<Result<List<CartItemDto>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            var query =await _repository.GetAllAsQuerable();

            var products = await query.Include(x=>x.OptionCombinations).
                Where(p => request.ProductIds.Contains(p.Id)).ToListAsync(cancellationToken);
            

           var cartItem= products.Select( x => new CartItemDto(_currencyService)
            {
               
                ProductId = x.Id,
                ProductName = x.Name.name,
                ProductPrice = x.Price,
                CalculatedProductPrice = _productPricingService.
                CalculateProductPrice(x.Price,x.OldPrice,x.SpecialPrice
                ,x.SpecialPriceStart,x.SpecialPriceEnd),
                ProductStockQuantity = x.StockQuantity,
                ProductStockTrackingIsEnabled = x.StockTrackingIsEnabled,
                IsProductAvailabeToOrder = x.IsAllowToOrder && x.IsPublished && !x.IsDeleted,
                ProductImage =  GetProductImage(x.Id).Result,
                Quantity = x.StockQuantity,
                VariationOptions = x.OptionCombinations.Select(oc => new ProductVariationOptionDto
                {
                    OptionName = oc.Option.Name,
                    OptionValue = oc.Value
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
