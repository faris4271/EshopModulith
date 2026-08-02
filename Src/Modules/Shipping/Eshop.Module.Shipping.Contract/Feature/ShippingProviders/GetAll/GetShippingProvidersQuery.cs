using Eshop.Module.Shipping.Contract.Dtos;
using Shared.Contract.CQRS;

namespace Eshop.Module.Shipping.Contract.Feature.ShippingProviders.GetAll;

public sealed record GetShippingProvidersQuery : IQuery<List<ShippingProviderDto>>;
