using Shared.Contract.CQRS;

namespace Eshop.Module.Core.Feature.Medias.DeletMedias
{
    public record DeletMediaCommand(List<Guid> Ids) : ICommand;

}
