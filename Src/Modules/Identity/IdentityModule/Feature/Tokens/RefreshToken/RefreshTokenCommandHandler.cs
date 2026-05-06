using IdentityModule.Services;
using Module.Identity.Contract.Feature.Tokens.RefreshToken;
using Module.Identity.Contract.Services;
using Shared.Contract.Context;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityModule.Feature.Tokens.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
    {

        private readonly IIdentityService _identityService;
        private readonly ITokenServic _tokenService;
        private readonly IRequestContext _requestContext;
        private readonly ISessionService _sessionService;

   

        public RefreshTokenCommandHandler(IIdentityService identityService, ITokenServic tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
          

        }

        public async Task<Result<RefreshTokenCommandResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var ip = _requestContext.IpAddress;

            var clientId = _requestContext.ClientId;

            var userAgent = _requestContext.UserAgent;


            if (request == null)
                return Result.Failure<RefreshTokenCommandResponse>(Error.NullValue);

            var validate =await _identityService
                .ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (validate == null)
                throw new UnauthorizedAccessException();

            var (subject,refreshToken)=validate.Value;


            var refreshTokenHash = Sha256Short(request.RefreshToken);

            var validateSession = await _sessionService.ValidateSessionAsync( refreshTokenHash, cancellationToken);

            if(!validateSession)
                throw new UnauthorizedAccessException("refreshToken not valid");

            var handler=new JwtSecurityTokenHandler();

            JwtSecurityToken parseAccessTokent = null;

            try
            {
                parseAccessTokent = handler.ReadJwtToken(request.Token);
            }
            catch 
            {
                
            }

            if (parseAccessTokent != null)
            {
                var accessSubject=parseAccessTokent.Claims.
                    FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)?.Value;

                if(!string.IsNullOrEmpty(accessSubject)&&
                    !string.Equals(accessSubject,subject,
                    StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("access token not valid for this refresh token");
            }

            var newAccessToken = await _tokenService.IssueAsync(subject, parseAccessTokent.Claims, cancellationToken);

            

          await  _identityService.StoreRefreshTokenAsync(
              subject, newAccessToken.RefreshToken, 
              newAccessToken.RefreshTokenExpiresAt, cancellationToken);

            var newRefreshTokenHashed = Sha256Short(newAccessToken.RefreshToken);

            var userSession=  await _sessionService.CreateSessionAsync(
               clientId,newRefreshTokenHashed,ip,userAgent
               ,newAccessToken.RefreshTokenExpiresAt, cancellationToken);



            var result= new RefreshTokenCommandResponse
            (
                newAccessToken.AccessToken,
              newAccessToken.RefreshToken,
               newAccessToken.RefreshTokenExpiresAt
            );

            return Result.Success(result);
        }

        private static string Sha256Short(string value)
        {
            var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(hash.AsSpan(0, 8));
        }
    }
}
