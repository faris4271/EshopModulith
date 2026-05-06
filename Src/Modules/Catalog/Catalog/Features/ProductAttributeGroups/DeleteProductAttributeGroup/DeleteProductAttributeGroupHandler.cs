using Catalog.Data;
using Catalog.Products.Models;
using MediatR;
using Shared.Abstraction;
using Shared.Contract;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductAttributeGroups.DeleteProductAttributeGroup;

public sealed class DeleteProductAttributeGroupHandler(IGenericeRepository<ProductAttributeGroup,CatalogDbContext> repo)
    : ICommandHandler<DeleteProductAttributeGroupCommand>
{
    public async Task<Result> Handle(DeleteProductAttributeGroupCommand cmd, CancellationToken ct)
    {
        var group = await repo.GetByIdAsync(cmd.Id, ct);
        if (group == null) return Result.Failure(Error.NotFound("404", "Product attribute group not found."));

         repo.Delete(group);
        return Result.Success();
    }
}
