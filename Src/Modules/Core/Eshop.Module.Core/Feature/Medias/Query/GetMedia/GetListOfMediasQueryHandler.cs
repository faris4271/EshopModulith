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
    public class GetListOfMediasQueryHandler(IGenericeRepository<Media,CoreDbContext> _repository) : IQueryHandler<GetListOfMediasQuery, List<MediaDto>>
    {
        public async Task<Result<List<MediaDto>>> Handle(GetListOfMediasQuery request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAllAsQuerable();

            var media = query.Where(m => request.Medias.Contains(m.Id))
                .Select(m => new MediaDto(m.Id, m.FileName)).ToList();

            return Result.Success(media);
        }
    }
}
