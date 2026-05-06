using Catalog.Data;
using Catalog.Products.Models;
using MediatR;
using Shared.Abstraction;
using Shared.Contract;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.DDD;

namespace Catalog.Features.ProductAttributeGroups.CreateProductAttributeGroup;

public sealed class CreateProductAttributeGroupHandler(IGenericeRepository<ProductAttributeGroup,CatalogDbContext> repo)
    : ICommandHandler<CreateProductAttributeGroupCommand>
{
    public async Task<Result> Handle(CreateProductAttributeGroupCommand cmd, CancellationToken ct)
    {
        var group = new ProductAttributeGroup
        {
            Name = new Name(cmd.Name)
        };

        await repo.AddAsync(group, ct);
        return Result.Success();
    }
}
