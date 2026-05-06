
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Roles.GetRole;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityModule.Feature.Roles.GetRoleById;

public sealed class GetRoleByIdQueryHandler : IQueryHandler<GetRoleQuery, RoleDto?>
{
    private readonly IRoleService _roleService;

    public GetRoleByIdQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Result<RoleDto?>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
    {
        if (request == null) 
            return Result.Failure<RoleDto?>( Error.Failure("400","Request cannot be null."));

        var result= await _roleService.
            GetRoleAsync(request.Id, cancellationToken).ConfigureAwait(false);

        return Result.Success( result );
    }
}
