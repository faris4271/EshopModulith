using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;
using System;


namespace EShop.Module.Core.Contract.Feature.Widgets.UpdateWidgetInstance
{
    public record UpdateWidgetInstanceCommand(Guid Id, WidgetBaseDto Widget) : ICommand;
  
}
