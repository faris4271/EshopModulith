using EShop.Module.Core.Contract.Dtos;
using Microsoft.AspNetCore.Http;
using Shared.Contract.CQRS;

namespace EShop.Module.Core.Contract.Feature.Medias.CreatMedia
{
    public record CreatMediaCommand(IFormFileCollection files, string Caption = null) : ICommand<List<MediaDto>>;

}
