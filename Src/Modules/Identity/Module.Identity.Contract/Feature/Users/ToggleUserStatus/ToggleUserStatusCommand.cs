
using MediatR;
using Shared.Contract.CQRS;

namespace Module.Identity.Contract.Feature.Users.ToggleUserStatus;

public class ToggleUserStatusCommand : ICommand
{
    public bool ActivateUser { get; set; }
    public string? UserId { get; set; }
}
