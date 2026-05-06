using MediatR;
using Shared.Contract;
using Shared.Contract.CQRS;

namespace Catalog.Features.ProductAttributeGroups.UpdateProductAttributeGroup;

public sealed record UpdateProductAttributeGroupCommand(Guid Id, string Name) : ICommand;
