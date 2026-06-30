using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Feature.Medias.DeletMedias;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Storage.Services;

namespace Eshop.Module.Core.Feature.Medias.DeletMedias
{
    public class DeletMediaCommandHandler(IGenericeRepository<Media, CoreDbContext> _repository, IStorageService _storageService) : ICommandHandler<DeletMediaCommand>
    {
        public async Task<Result> Handle(DeletMediaCommand request, CancellationToken cancellationToken)
        {
            var query = await _repository.Query();

            var medias = await query.Where(x => request.Ids.Contains(x.Id)).ToListAsync();

            if (medias.Any())
            {
                _repository.DeleteRange(medias);


                foreach (var media in medias)
                {
                    if (await _storageService.ExistsAsync(media.FileName, cancellationToken))
                    {
                        await _storageService.RemoveAsync(media.FileName, cancellationToken);
                    }

                }
            }
            return Result.Success();
        }
    }

}
