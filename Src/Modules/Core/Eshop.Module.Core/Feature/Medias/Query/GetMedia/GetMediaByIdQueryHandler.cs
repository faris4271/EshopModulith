using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Medias.GetMedia;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Core.Feature.Medias.Query.GetMedia
{
    internal class GetMediaByIdQueryHandler(IGenericeRepository<Media,CoreDbContext> _repository) : IQueryHandler<GetMediaByIdQuery, List<MediaDto>>
    {
        public async Task<Result<List<MediaDto>>> Handle(GetMediaByIdQuery request, CancellationToken cancellationToken)
        {
            var query =await _repository.GetAllAsQuerable();

            var media = query.Where(m => m.Id == request.id).Select(m => new MediaDto (  m.Id,m.FileName )).ToList();


            if (media.Count<=0)
                return Result.Failure<List<MediaDto>>(
                    new Error("not found","Media not found",ErrorType.NotFound));

            return Result.Success(media);
        }
    }
}
