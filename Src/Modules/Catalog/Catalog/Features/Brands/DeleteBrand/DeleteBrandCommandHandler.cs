using Catalog.Brands.Moddels;
using Catalog.Data;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.Brands.DeleteBrand
{
    internal class DeleteBrandCommandHandler(
        IGenericeRepository<Brand, CatalogDbContext> _repository,
        IEntityService _entityService
        ) : ICommandHandler<DeleteBrandCommand, Guid>
    {
        const string EntityTypeId = "Brand";
        public async Task<Result<Guid>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _repository.BeginTransaction())
            {
                try
                {
                    _repository.Delete(request.id);

                    await _entityService.Remove(request.id, EntityTypeId);

                    transaction.Commit();
                    var res = await _repository.SaveChangesAsync();

                    return Result.Success(Guid.NewGuid());
                }
                catch (Exception ex)
                {
                    return Result.Failure<Guid>(Error.Problem("400", ex.Message));

                    transaction.Rollback();
                }
            }


        }
    }
}
