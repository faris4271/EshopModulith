using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Abstraction;
using Shared.Contract;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroups;

public sealed class GetProductAttributeGroupsHandler(IGenericeRepository<ProductAttributeGroup, CatalogDbContext> repo)
    : IQueryHandler<GetProductAttributeGroupsQuery, IEnumerable<ProductAttributeGroupDto>>
{
    public async Task<Result<IEnumerable<ProductAttributeGroupDto>>> Handle(GetProductAttributeGroupsQuery query, CancellationToken ct)
    {
        var groups = await repo.GetAllAsync(ct);
        var result= groups.Select(g => new ProductAttributeGroupDto
        {
            Id = g.Id,
            Name = g.Name.name
        });
        return Result.Success(result);
    }
}
