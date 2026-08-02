using Shared.Contract.CQRS;

namespace Eshop.Module.Shipping.Contract.Feature.ShippingProviders.DisableShippingProvider;

public sealed record DisableShippingProviderCommand(Guid Id) : ICommand;
