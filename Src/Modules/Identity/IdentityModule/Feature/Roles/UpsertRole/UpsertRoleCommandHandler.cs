using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Roles.UpsertRole;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Roles.UpsertRole;

public sealed class UpsertRoleCommandHandler : ICommandHandler<UpsertRoleCommand, RoleDto>
{
    private readonly IRoleService _roleService;

    public UpsertRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Result<RoleDto>> Handle(UpsertRoleCommand request, CancellationToken cancellationToken)
    {
       if(request == null) 
           return Result.Failure<RoleDto>(Error.Failure("400", "Request cannot be null."));

        var result= await _roleService.CreateOrUpdateRoleAsync(request.Id, request.Name, request.Description ?? string.Empty, cancellationToken)
            .ConfigureAwait(false);

     return Result.Success(result);
    }
}
