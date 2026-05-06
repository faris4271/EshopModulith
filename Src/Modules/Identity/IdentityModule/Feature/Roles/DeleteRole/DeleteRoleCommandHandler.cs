using Module.Identity.Contract.Feature.Roles.DeleteRole;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Roles.DeleteRole;

public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        if (request == null) 
            return Result.Failure( Error.Failure("400","Request cannot be null."));

         await _roleService.DeleteRoleAsync(request.Id, cancellationToken).ConfigureAwait(false);

        return Result.Success();

    }
}
