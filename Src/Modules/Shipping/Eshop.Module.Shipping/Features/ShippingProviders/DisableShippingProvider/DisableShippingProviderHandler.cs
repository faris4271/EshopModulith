using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.DisableShippingProvider;
using Eshop.Module.Shipping.Data;
using Eshop.Module.Shipping.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Shipping.Features.ShippingProviders.DisableShippingProvider;

internal sealed class DisableShippingProviderHandler(
    IGenericeRepository<ShippingProvider, ShippingDbContext> repository)
    : ICommandHandler<DisableShippingProviderCommand>
{
    public async Task<Result> Handle(DisableShippingProviderCommand command, CancellationToken ct)
    {
        var provider = await repository.GetByIdAsync(command.Id, ct);
        if (provider is null)
            return Result.Failure(Error.NotFound("Shipping.Provider.NotFound", "Shipping provider was not found"));

        provider.SetEnabled(false);
        repository.Update(provider);
        await repository.SaveChangesAsync(ct);

        return Result.Success();
    }
}
