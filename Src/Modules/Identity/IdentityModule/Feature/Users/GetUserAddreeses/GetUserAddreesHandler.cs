using EShop.Module.Core.Contract.Feature.GetAddress;
using IdentityModule.Data;
using IdentityModule.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Users.GetUserAddrees;
using Module.Identity.Contract.Services;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Users.GetUserAddreeses
{
    internal class GetUserAddreesHandler(
        ICurrentUserService _currentUserService,
        IGenericeRepository<UserAddress, IdentityDbContext> _repository,
        ISender _sender)
        : IQueryHandler<GetUserAddreesQuery, GetUserAddressDto>
    {
        public async Task<Result<GetUserAddressDto>> Handle(GetUserAddreesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();

            if (userId != request.userAddressId)
            {
                return Result.Failure<GetUserAddressDto>(new Error("Unauthorized", "You are not authorized to access this resource.", ErrorType.Unauthorized));
            }

            var query = await _repository.Query();

            var addressIds = await query.Where(x => x.UserId == userId.ToString()).Select(x => x.AddressId).ToListAsync(cancellationToken);

            var addresses = await _sender.Send(new GetAddressByIdsQuery(addressIds), cancellationToken);



            if (!addresses.IsSuccess)
            {
                return Result.Failure<GetUserAddressDto>(addresses.Error);
            }

            var result = new GetUserAddressDto
            {
                UserId = userId,
                Addresses = addresses.Value
            };

            return Result.Success(result);
        }
    }
}
