using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.CreatMedia;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Storage;
using Shared.Storage.Services;
using System.Net.Http.Headers;

namespace Eshop.Module.Core.Feature.Medias.Command.CreatMedia
{
    public class CreatMediaCommandHandler(IGenericeRepository<Media, CoreDbContext> _repository, IStorageService _storageService) : ICommandHandler<CreatMediaCommand, List<MediaDto>>

    {
        public async Task<Result<List<MediaDto>>> Handle(CreatMediaCommand request, CancellationToken cancellationToken)
        {
            var ms = new MemoryStream();
        

         var fileRequestList= new List<MediaDto>();
            
            var Medias=new List<Media>();

            foreach (var file in request.files)
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
                     request.Caption ?? fileName,
                     fileName,
                    file.ContentType
                     );
                Medias.Add(media);

              var mediaDto = new MediaDto(media.Id, fileName);
                fileRequestList.Add(mediaDto);

            }
            await _repository.AddRangeAsync(Medias);
           await _repository.SaveChangesAsync();

            
            return Result.Success(fileRequestList);
        }

    }
}
