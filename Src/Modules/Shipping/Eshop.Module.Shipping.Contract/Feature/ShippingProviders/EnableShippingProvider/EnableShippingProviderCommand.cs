using Shared.Contract.CQRS;

namespace Eshop.Module.Shipping.Contract.Feature.ShippingProviders.EnableShippingProvider;

public sealed record EnableShippingProviderCommand(Guid Id) : ICommand;
