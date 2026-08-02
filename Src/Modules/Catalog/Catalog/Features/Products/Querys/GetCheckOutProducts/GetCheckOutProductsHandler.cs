using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using CatalogContract.Features.GetCheckOutProducts;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using EShop.Module.Core.Contract.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Products.Querys.GetCheckOutProducts
{
    internal class GetCheckOutProductsHandler(IGenericeRepository<Product, CatalogDbContext> _repository, ISender _sender, ICurrencyService _currencyService) : IQueryHandler<GetCheckOutProductsQuery, List<CheckoutItemDto>>
    {
        public async Task<Result<List<CheckoutItemDto>>> Handle(GetCheckOutProductsQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            query = query.Include(x => x.OptionCombinations).Include(x => x.OptionValues).ThenInclude(x => x.Option)
               .Where(x => request.productIds.Contains(x.Id));

            var mediaIds = query.Select(x => x.MainImageId).ToList();

            var mediaResults = await _sender.Send(new GetListOfMediasQuery(mediaIds), cancellationToken);

            if (!mediaResults.IsSuccess)
            {
                return Result.Failure<List<CheckoutItemDto>>(mediaResults.Error);
            }
            var mediaDictionary = mediaResults.Value.ToDictionary(x => x.id, x => x.FileName);

            var products = await query
                .Select(x => new CheckoutItemDto
                {
                    Id = x.Id,
                    ProductId = x.Id,
                    ProductName = x.Name.name,
                    ProductPrice = x.Price.Amount,
                    ProductStockQuantity = x.StockQuantity,
                    ProductStockTrackingIsEnabled = x.StockTrackingIsEnabled,
                    IsProductAvailabeToOrder = x.IsAllowToOrder,
                    Quantity = 1, // Default quantity for checkout
                    ProductPriceString = _currencyService.FormatCurrency(x.Price),

                    ProductImage = mediaDictionary.ContainsKey(x.MainImageId) ? mediaDictionary[x.MainImageId] : null,
                    VariationOptions = x.OptionCombinations.Select(o => new ProductVariationOptionDto
                    {

                        OptionName = o.Option.Name,
                        Value = o.Value
                    }).ToList()
                }).ToListAsync(cancellationToken);

            return Result.Success(products);
        }
    }
}
