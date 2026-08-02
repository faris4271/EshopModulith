using Eshop.Module.Shipping.Contract.Feature.ShippingProviders.DisableShippingProvider;
using FluentValidation;

namespace Eshop.Module.Shipping.Features.ShippingProviders.DisableShippingProvider;

internal sealed class DisableShippingProviderValidator : AbstractValidator<DisableShippingProviderCommand>
{
    public DisableShippingProviderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
