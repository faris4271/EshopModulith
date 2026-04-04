using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias;
using EShop.Module.Core.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System.Net.Http.Headers;

namespace Eshop.Module.Core.Feature.Medias.Command.CreatMedia
{
    public class CreatMediaCommandHandler(IGenericeRepository<Media, CoreDbContext> _repository, IStorageService _storageService) : ICommandHandler<CreatMediaCommand, MediaDto>

    {
        public async Task<Result<MediaDto>> Handle(CreatMediaCommand request, CancellationToken cancellationToken)
        {
            var file = request.file;
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";

            var filePath = await _storageService.SaveMediaAsync(file.OpenReadStream(), fileName, file.ContentType);

            var media = new Media(

                (int)file.Length,
                request.Caption ?? fileName,
                fileName,
                file.ContentType

                );


            _repository.Add(media);

            var mediaDto = new MediaDto(media.Id, fileName);
            return Result.Success(mediaDto);
        }

    }
}
