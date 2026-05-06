using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module.Identity.Contract.Feature.Tokens.RefreshToken
{
    public record RefreshTokenCommand(string Token , string RefreshToken) : ICommand<RefreshTokenCommandResponse>;
    
}
