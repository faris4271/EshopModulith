using Shared.Contract.ResultPattern;

namespace Module.Inventory.Services
{
    internal interface IStockService
    {
        Task<Error> UpdateStockAsync(Guid stockId, int adjustedQuantity, string note, CancellationToken cancellationToken);
    }
}
