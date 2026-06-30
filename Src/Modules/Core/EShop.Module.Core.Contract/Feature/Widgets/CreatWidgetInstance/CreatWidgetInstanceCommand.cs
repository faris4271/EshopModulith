using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;

namespace EShop.Module.Core.Contract.Feature.Widgets.CreatWidgetInstance
{

    public record CreatWidgetInstanceCommand(WidgetBaseDto Widget) : ICommand;
   
}
