using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.UpdateMedia;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Storage;
using Shared.Storage.Services;
using System.Net.Http.Headers;

namespace Eshop.Module.Core.Feature.Medias.Command.UpdateMedia
{
    internal class UpdateMediaCommandHandler(IGenericeRepository<Media, CoreDbContext> _repository, IStorageService _storageService) : ICommandHandler<UpdateMediaCommand, List<MediaDto>>
    {
        public async Task<Result<List<MediaDto>>> Handle(UpdateMediaCommand request, CancellationToken cancellationToken)
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
            var ms = new MemoryStream();


            var fileRequestList = new List<MediaDto>();

            var Medias = new List<Media>();

            foreach (var file in request.Files)
            {
                if (file.Length > 0)
                {
                    await file.CopyToAsync(ms, cancellationToken);

                    var fileRequest = new FileUploadRequest
                    {
                        FileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'),
                        ContentType = file.ContentType,
                        Data = new List<byte>(ms.ToArray())
                    };
                    var fileName = await _storageService.UploadAsync<Media>(fileRequest, FileType.Image, cancellationToken);
                    var media = new Media(
                        fileRequest.Data.Count,
                          "fileName",
                         fileName,
                        file.ContentType
                         );
                    Medias.Add(media);

                    var mediaDto = new MediaDto(media.Id, fileName);
                    fileRequestList.Add(mediaDto);
                }

            }
            await _repository.AddRangeAsync(Medias);
            await _repository.SaveChangesAsync();

            return Result.Success(fileRequestList);





        }
    }
}
