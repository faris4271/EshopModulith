using EShop.Module.Core.Contract.Dtos;
using Microsoft.AspNetCore.Http;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Module.Core.Contract.Feature.Medias.UpdateMedia
{
    public record UpdateMediaCommand(List<Guid> Ids , IFormFileCollection Files): ICommand<List<MediaDto>>;

}
