using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.ResetTenantTheme;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.ResetTenantTheme
{
    internal sealed class ResetTenantThemeCommandHandler(
        ITenantThemeService themeService,
        ILogger<ResetTenantThemeCommandHandler> logger)
        : ICommandHandler<ResetTenantThemeCommand>
    {
        public async Task<Result> Handle(ResetTenantThemeCommand request, CancellationToken cancellationToken)
        {
            await themeService.ResetThemeAsync(request.TenantId, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant theme reset: {TenantId}", request.TenantId);

            return Result.Success();
        }
    }
}