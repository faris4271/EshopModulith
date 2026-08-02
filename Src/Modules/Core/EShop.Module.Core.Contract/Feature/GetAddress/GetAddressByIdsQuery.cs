using EShop.Module.Core.Contract.Dtos;
using Shared.Contract.CQRS;

namespace EShop.Module.Core.Contract.Feature.GetAddress
{
    public record GetAddressByIdsQuery(List<Guid> AddressIds) : IQuery<List<ShippingAddressDto>>;


}
