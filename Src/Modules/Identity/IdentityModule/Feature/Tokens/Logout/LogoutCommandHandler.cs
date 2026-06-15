using Module.Identity.Contract.Feature.Tokens.Logout;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace IdentityModule.Feature.Tokens.Logout
{
    internal class LogoutCommandHandler : ICommandHandler<LogoutCommand>
    {
        public Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
