
using Module.Identity.Contract.Feature.Roles.UpdatePermissions;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Roles.UpdateRolePermissions;

public sealed class UpdatePermissionsCommandHandler : ICommandHandler<UpdatePermissionsCommand, string>
{
    private readonly IRoleService _roleService;

    public UpdatePermissionsCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Result<string>> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
    {
        if(request == null) 
            return Result.Failure<string>(Error.Failure("400", "Request cannot be null."));
        
        var result= await _roleService.UpdatePermissionsAsync(request.RoleId,
            request.Permissions, cancellationToken).ConfigureAwait(false);

        return Result.Success(result);
    }
}
