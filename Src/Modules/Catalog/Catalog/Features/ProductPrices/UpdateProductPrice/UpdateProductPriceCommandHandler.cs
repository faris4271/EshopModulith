using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Catalog.Features.ProductPrices.UpdateProductPrice
{
    internal class UpdateProductPriceCommandHandler : ICommandHandler<UpdateProductPriceCommand>
    {
        public Task<Result> Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
