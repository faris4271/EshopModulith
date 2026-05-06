using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Module.Core.Contract.Feature.Medias.GetMedia
{
    public record GetListOfMediasQuery(List<Guid> Medias) : IQuery<List<MediaDto>>;

}
