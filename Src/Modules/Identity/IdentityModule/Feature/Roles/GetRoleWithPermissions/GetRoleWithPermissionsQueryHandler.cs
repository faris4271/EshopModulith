using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Roles.GetRoleWithPermissions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Roles.GetRoleWithPermissions;

public sealed class GetRoleWithPermissionsQueryHandler : IQueryHandler<GetRoleWithPermissionsQuery, RoleDto>
{
    private readonly IRoleService _roleService;

    public GetRoleWithPermissionsQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Result<RoleDto>> Handle(GetRoleWithPermissionsQuery query, CancellationToken cancellationToken)
    {
        if(query == null) 
            return Result.Failure<RoleDto>( Error.Failure("400","Query cannot be null."));

        var result= await _roleService.GetWithPermissionsAsync(query.Id, cancellationToken).ConfigureAwait(false);

      return  Result.Success(result);
    }
}
