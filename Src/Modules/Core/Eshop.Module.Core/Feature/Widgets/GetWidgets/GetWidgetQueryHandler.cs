using Eshop.Module.Core.Data;
using Eshop.Module.Core.Models;
using EShop.Module.Core.Contract.Dtos;
using EShop.Module.Core.Contract.Feature.Widgets.GetWidgets;
using Mapster;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Module.Core.Feature.Widgets.GetWidgets
{
    internal class GetWidgetQueryHandler(IGenericeRepository<Widget, CoreDbContext> repository) : IQueryHandler<GetWidgetQuery, List<WidgetDto>>
    {
        public async Task<Result<List<WidgetDto>>> Handle(GetWidgetQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.GetAllAsync();

            var dtos = result.Adapt<List<WidgetDto>>();
            return Result.Success(dtos);
        }
    }
}
