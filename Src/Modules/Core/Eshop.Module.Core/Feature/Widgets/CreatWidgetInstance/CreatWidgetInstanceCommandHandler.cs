using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Feature.Widgets.CreatWidgetInstance;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Eshop.Module.Core.Feature.Widgets.CreatWidgetInstance
{
    internal class CreatWidgetInstanceCommandHandler(IGenericeRepository<WidgetInstance,CoreDbContext> _repository) : ICommandHandler<CreatWidgetInstanceCommand>
    {
        public async Task<Result> Handle(CreatWidgetInstanceCommand request, CancellationToken cancellationToken)
        {
            var widgetInstance = new WidgetInstance
            {
                Name = request.Widget.Name,
                PublishStart = request.Widget.PublishStart,
                WidgetId= request.Widget.WidgetId,
                WidgetZoneId = request.Widget.WidgetZoneId,
                PublishEnd = request.Widget.PublishEnd,
                DisplayOrder= request.Widget.DisplayOrder,
                Data= JsonConvert.SerializeObject(request.Widget.Settings),
            };

           await _repository.AddAsync(widgetInstance);

            await _repository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
