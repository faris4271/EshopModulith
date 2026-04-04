

using EShop.Module.Core.Contract.Dtos;
using Microsoft.AspNetCore.Http;
using Shared.Contract.CQRS;

namespace EShop.Module.Core.Contract.Feature.Medias
{
    public record CreatMediaCommand(IFormFile file, string Caption = null) : ICommand<MediaDto>;

}
