using Eshop.Module.Basket.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Commands.DeletItems
{
    internal class DeleteItemCommandHandler(ICartService _cartService) : ICommandHandler<DeleteItemCommand>
    {
        public async Task<Result> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            await _cartService.RemoveItem(request.CustomerId, request.ProductId);

            return Result.Success();
        }
    }
}
