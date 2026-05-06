using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Feature.Widgets.UpdateWidgetInstance;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Core.Feature.Widgets.UpdateWidgetInstance
{
    internal class UpdateWidgetInstanceCommandHandler(
        IGenericeRepository<WidgetInstance,CoreDbContext> _repository) 
        : ICommandHandler<UpdateWidgetInstanceCommand>
    {
        public async Task<Result> Handle(UpdateWidgetInstanceCommand model, CancellationToken cancellationToken)
        {
            var widgetInstance =await _repository.GetByIdAsync(model.Id, cancellationToken);

            if (widgetInstance == null) 
                return Result.Failure(Error.NullValue);

            widgetInstance.Name = model.Widget.Name;
            widgetInstance.WidgetZoneId = model.Widget.WidgetZoneId;
            widgetInstance.PublishStart = model.Widget.PublishStart;
            widgetInstance.PublishEnd = model.Widget.PublishEnd;
            widgetInstance.DisplayOrder = model.Widget.DisplayOrder;
            widgetInstance.Data = JsonConvert.SerializeObject(model.Widget.Settings);

             _repository.Update(widgetInstance);

            await _repository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
