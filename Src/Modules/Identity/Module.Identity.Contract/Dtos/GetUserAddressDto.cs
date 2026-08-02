using EShop.Module.Core.Contract.Dtos;

namespace Module.Identity.Contract.Dtos
{
    public record GetUserAddressDto
    {
        public Guid UserId { get; init; }

        public List<ShippingAddressDto> Addresses { get; init; }

    }


}
