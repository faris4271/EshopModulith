using Eshop.Module.Basket.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace Eshop.Module.Basket.Feature.Commands.UpdateQuantity
{
    internal class UpdateQuantityCommandHandler(ICartService _cartService) : ICommandHandler<UpdateQuantityCommand>
    {
        public async Task<Result> Handle(UpdateQuantityCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartService.UpdateQuantity(request.CartQuantityUpdateDto);

            if (!result.Success)
            {
                return Result.Failure(Error.Failure("400", result.ErrorMessage));
            }

            return Result.Success();
        }
    }
}
