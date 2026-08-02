using Eshop.Module.Shipping.Contract.Dtos;
using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.GetAll;
using Eshop.Module.Shipping.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.ShippingProviders.GetAll;

internal sealed class GetShippingProvidersHandler(
    IGenericeRepository<Models.ShippingProvider, ShippingDbContext> repository)
    : IQueryHandler<GetShippingProvidersQuery, List<ShippingProviderDto>>
{


    async Task<Result<List<ShippingProviderDto>>> IRequestHandler<GetShippingProvidersQuery, Result<List<ShippingProviderDto>>>.Handle(GetShippingProvidersQuery request, CancellationToken cancellationToken)
    {
        var query = await repository.Query();
        var providers = await query
            .Select(p => new ShippingProviderDto(
                p.Id,
                p.Name.name,
                p.IsEnabled,
                p.ConfigureUrl))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success(providers);
    }
}
