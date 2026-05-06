using CatalogContract.Dtos;
using MediatR;
using Shared.Contract;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroupById;

public sealed record GetProductAttributeGroupByIdQuery(Guid Id) : IQuery<ProductAttributeGroupDto>;
