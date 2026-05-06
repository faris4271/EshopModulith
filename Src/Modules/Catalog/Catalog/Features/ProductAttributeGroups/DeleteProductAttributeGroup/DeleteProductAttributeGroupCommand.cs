using MediatR;
using Shared.Contract;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributeGroups.DeleteProductAttributeGroup;

public sealed record DeleteProductAttributeGroupCommand(Guid Id) : ICommand;
