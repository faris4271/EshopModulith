using CatalogContract.Dtos;
using MediatR;
using Shared.Contract;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributeGroups.GetProductAttributeGroups;

public sealed record GetProductAttributeGroupsQuery() : IQuery<IEnumerable<ProductAttributeGroupDto>>;
