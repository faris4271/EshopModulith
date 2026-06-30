using Shared.Contract.CQRS;

namespace EShop.Module.Core.Contract.Feature.Medias.DeletMedias
{
    public record DeletMediaCommand(List<Guid> Ids) : ICommand;

}
