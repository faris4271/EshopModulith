using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.SetDefaultTenantTheme;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.SetDefaultTenantTheme
{
    internal sealed class SetDefaultTenantThemeCommandHandler(
        ITenantThemeService themeService,
        ILogger<SetDefaultTenantThemeCommandHandler> logger)
        : ICommandHandler<SetDefaultTenantThemeCommand>
    {
        public async Task<Result> Handle(SetDefaultTenantThemeCommand request, CancellationToken cancellationToken)
        {
            await themeService.SetAsDefaultThemeAsync(request.TenantId, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant theme set as default: {TenantId}", request.TenantId);

            return Result.Success();
        }
    }
}