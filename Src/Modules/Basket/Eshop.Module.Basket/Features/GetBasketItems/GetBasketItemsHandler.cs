//using Eshop.Module.Basket.Contract.Dtos;
//using Eshop.Module.Basket.Contract.Features.GetBasketItems;
//using Eshop.Module.Basket.Data;
//using Eshop.Module.Basket.Models;
//using Shared.Abstraction;
//using Shared.Contract.CQRS;
//using Shared.Contract.ResultPattern;

//namespace Eshop.Module.Basket.Features.GetBasketItems
//{
//    internal class GetBasketItemsHandler(
//        IGenericeRepository<CartItem, BasketDbContext> repository)
//        : IQueryHandler<GetBasketItemsQuery, List<BasketItemDto>>
//    {
//        public async Task<Result<List<BasketItemDto>>> Handle(
//            GetBasketItemsQuery request,
//            CancellationToken ct)
//        {
//            var query = await repository.GetAllAsQuerable();
//            var items = await query

//                .Where(i => i.Id == request.BasketId)
//                .Select(i => new BasketItemDto(i.Id, i.ProductId, i.Quantity))
//                .ToListAsync(ct);

//            return Result.Success(items);
//        }
//    }
//}