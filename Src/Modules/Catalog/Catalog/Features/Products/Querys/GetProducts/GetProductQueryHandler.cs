using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace Catalog.Features.Products.Querys.GetProducts
{
    public record GetProductQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<ProductListItemDto>>;

    internal record GetProductQueryResponse();
    internal class GetProductQueryHandler(IGenericeRepository<Product, CatalogDbContext> _repository) : IQueryHandler<GetProductQuery, SmartTableResult<ProductListItemDto>>
    {
        public async Task<Result<SmartTableResult<ProductListItemDto>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            query = query.Include(x => x.AttributeValues);

            var param = request.SmartTableParam;
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Name != null)
                {
                    string name = search.Name;
                    query = query.Where(x => x.Name.name.Contains(name));
                }

                if (search.HasOptions != null)
                {
                    bool hasOptions = search.HasOptions;
                    query = query.Where(x => x.HasOptions == hasOptions);
                }

                if (search.IsVisibleIndividually != null)
                {
                    bool isVisibleIndividually = search.IsVisibleIndividually;
                    query = query.Where(x => x.IsVisibleIndividually == isVisibleIndividually);
                }

                if (search.IsPublished != null)
                {
                    bool isPublished = search.IsPublished;
                    query = query.Where(x => x.IsPublished == isPublished);
                }
                if (search.IsFeatured != null)
                {
                    bool IsFeatured = search.IsFeatured;
                    query = query.Where(x => x.IsFeatured == IsFeatured);
                }

                if (search.CreatedOn != null)
                {
                    if (search.CreatedOn.before != null)
                    {
                        DateTimeOffset before = search.CreatedOn.before;
                        query = query.Where(x => x.CreatedOn <= before);
                    }

                    if (search.CreatedOn.after != null)
                    {
                        DateTimeOffset after = search.CreatedOn.after;
                        query = query.Where(x => x.CreatedOn >= after);
                    }
                }
            }



            var gridData = query.ToSmartTableResult(
                param,
                x => new ProductListItemDto
                {
                    Id = x.Id,
                    Name = x.Name.name,
                    HasOptions = x.HasOptions,
                    IsVisibleIndividually = x.IsVisibleIndividually,
                    IsFeatured = x.IsFeatured,
                    IsAllowToOrder = x.IsAllowToOrder,
                    IsCallForPricing = x.IsCallForPricing,
                    StockQuantity = x.StockQuantity,
                    CreatedOn = x.CreatedOn,
                    IsPublished = x.IsPublished
                });

            return Result.Success(gridData);
        }
    }
}
