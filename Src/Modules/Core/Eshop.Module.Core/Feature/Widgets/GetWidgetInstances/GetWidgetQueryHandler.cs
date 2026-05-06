using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Widgets.GetProductWidgetInstances;
using Newtonsoft.Json;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Core.Feature.Widgets.GetWidgetInstances
{
    internal class GetWidgetQueryHandler(IGenericeRepository<WidgetInstance,CoreDbContext> _repository):IQueryHandler<GetWidgetInstanceQuery, WidgetBaseDto>
    {
        public async Task<Result<WidgetBaseDto>> Handle(GetWidgetInstanceQuery request, CancellationToken cancellationToken)
        {
            var widgetInstance =await _repository.GetByIdAsync(request.Id);

            if (widgetInstance == null)
            {
                return Result.Failure<WidgetBaseDto>(Error.Failure("404", "Widget instance not found."));
            }

            var widgetInstanceDto = new WidgetBaseDto
            {
                
                Name = widgetInstance.Name,
                WidgetId = widgetInstance.WidgetId,
                WidgetZoneId = widgetInstance.WidgetZoneId,
                PublishStart = widgetInstance.PublishStart,
                PublishEnd = widgetInstance.PublishEnd,
                DisplayOrder = widgetInstance.DisplayOrder,
                Settings = JsonConvert.SerializeObject(widgetInstance.Data)
            };

            return Result.Success(widgetInstanceDto);
        }
    }

}
