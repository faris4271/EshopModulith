using MediatR;
using Shared.Contract;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributeGroups.CreateProductAttributeGroup;

public sealed record CreateProductAttributeGroupCommand(string Name) : ICommand;
