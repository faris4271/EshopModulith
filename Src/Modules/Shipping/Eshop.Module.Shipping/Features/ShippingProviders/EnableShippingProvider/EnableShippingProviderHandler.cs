using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.EnableShippingProvider;
using Eshop.Module.Shipping.Data;
using Eshop.Module.Shipping.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.ShippingProviders.EnableShippingProvider;

internal sealed class EnableShippingProviderHandler(
    IGenericeRepository<ShippingProvider, ShippingDbContext> repository)
    : ICommandHandler<EnableShippingProviderCommand>
{
    public async Task<Result> Handle(EnableShippingProviderCommand command, CancellationToken ct)
    {
        var provider = await repository.GetByIdAsync(command.Id, ct);
        if (provider is null)
            return Result.Failure(Error.NotFound("Shipping.Provider.NotFound", "Shipping provider was not found"));

        provider.SetEnabled(true);

        await repository.UpdateAsync(provider, cancellationToken: ct);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
