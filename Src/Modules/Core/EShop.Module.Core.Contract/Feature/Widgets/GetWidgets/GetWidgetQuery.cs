using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Module.Core.Contract.Feature.Widgets.GetWidgets
{
    public record GetWidgetQuery : IQuery<List<WidgetDto>>;
 
}
