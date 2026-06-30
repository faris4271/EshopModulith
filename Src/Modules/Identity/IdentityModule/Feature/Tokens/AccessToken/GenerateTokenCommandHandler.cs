using Microsoft.Extensions.Logging;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Events;
using Module.Identity.Contract.Feature.Tokens.TokenGeneration;
using Module.Identity.Contract.Services;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Eventing.Contract;


namespace IdentityModule.Feature.Tokens.AccessToken
{
    internal class GenerateTokenCommandHandler : ICommandHandler<GenerateTokenCommand, TokenResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenServic _tokenService;
        private readonly ISessionService _sessionService;
        private readonly IRequestContextService _requestService;
        private readonly ILogger<GenerateTokenCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public GenerateTokenCommandHandler(IIdentityService identityService,
            ITokenServic tokenService, ISessionService sessionService,
            IRequestContextService requestService,
            ILogger<GenerateTokenCommandHandler> logger, IEventBus eventBus)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _sessionService = sessionService;
            _requestService = requestService;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task<Result<TokenResponse>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            var ip = _requestService.IpAddress ?? "unknowen";
            var ua = _requestService.UserAgent ?? "unknown";
            var clientId = _requestService.ClientId;

            var identityValue = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);

            var subject = identityValue.Value.Subject;

            var claims = identityValue.Value.Claims;

            var token = await _tokenService.IssueAsync(subject, claims, cancellationToken);

            await _identityService.StoreRefreshTokenAsync(subject, token.RefreshToken, token.RefreshTokenExpiresAt, cancellationToken);

            try
            {
                var refresgTokenHashed = Sha256Short(token.RefreshToken);

                await _sessionService.CreateSessionAsync(
                    subject, refresgTokenHashed,
                    ip, ua, token.RefreshTokenExpiresAt, cancellationToken);

            }
            catch (Exception ex)
            {

                _logger.LogWarning("cannot make session for this user");
            }

            var fingerprint = Sha256Short(token.AccessToken);


            var correlationId = Guid.NewGuid().ToString();

            var integrationEvent = new TokenGeneratedIntegrationEvent(
                Id: Guid.NewGuid(),
                OccurredOnUtc: DateTime.UtcNow,

                CorrelationId: correlationId,
                Source: "Identity",
                UserId: subject,
                Email: request.Email,
                ClientId: clientId!,
                IpAddress: ip,
                UserAgent: ua,
                TokenFingerprint: fingerprint,
                AccessTokenExpiresAtUtc: token.AccessTokenExpiresAt);

            await _eventBus.PublishAsync(integrationEvent);

            return Result.Success(token);

        }



        private static string Sha256Short(string value)
        {
            var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
            // short printable fingerprint; store only this
            return Convert.ToHexString(hash.AsSpan(0, 8));
        }
    }
}
