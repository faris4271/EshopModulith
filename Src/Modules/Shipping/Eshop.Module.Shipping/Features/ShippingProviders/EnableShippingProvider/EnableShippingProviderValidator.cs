using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.EnableShippingProvider;
using FluentValidation;

namespace Eshop.Module.Shipping.Features.ShippingProviders.EnableShippingProvider;

internal sealed class EnableShippingProviderValidator : AbstractValidator<EnableShippingProviderCommand>
{
    public EnableShippingProviderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
