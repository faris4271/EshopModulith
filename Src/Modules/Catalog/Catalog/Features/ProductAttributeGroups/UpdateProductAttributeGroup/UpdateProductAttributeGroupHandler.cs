using Catalog.Data;
using Catalog.Products.Models;
using MediatR;
using Shared.Abstraction;
using Shared.Contract;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.DDD;

namespace Catalog.Features.ProductAttributeGroups.UpdateProductAttributeGroup;

public sealed class UpdateProductAttributeGroupHandler(IGenericeRepository<ProductAttributeGroup,CatalogDbContext> repo)
    : ICommandHandler<UpdateProductAttributeGroupCommand>
{
    public async Task<Result> Handle(UpdateProductAttributeGroupCommand cmd, CancellationToken ct)
    {
        var group = await repo.GetByIdAsync(cmd.Id, ct);
        if (group == null) return Result.Failure(Error.NotFound("404", "Product attribute group not found."));

        group.Name = new Name(cmd.Name);
         repo.Update(group);
      await repo.SaveChangesAsync();
        return Result.Success();
    }
}
