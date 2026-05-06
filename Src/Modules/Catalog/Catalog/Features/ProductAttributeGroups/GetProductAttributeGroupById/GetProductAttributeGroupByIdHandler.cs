using Catalog.Data;
using Catalog.Products.Models;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Abstraction;
using Shared.Contract;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroupById;

public sealed class GetProductAttributeGroupByIdHandler(IGenericeRepository<ProductAttributeGroup, CatalogDbContext> repo)
    : IQueryHandler<GetProductAttributeGroupByIdQuery, ProductAttributeGroupDto>
{
    public async Task<Result<ProductAttributeGroupDto>> Handle(GetProductAttributeGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (group == null) return Result.Failure<ProductAttributeGroupDto>(Error.NotFound("404", "Product attribute group not found."));

        return new ProductAttributeGroupDto
        {
            Id = group.Id,
            Name = group.Name.name,
        };
    }
}
