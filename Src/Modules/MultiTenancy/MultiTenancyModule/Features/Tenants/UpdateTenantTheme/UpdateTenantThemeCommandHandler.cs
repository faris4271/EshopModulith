using MediatR;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.UpdateTenantTheme;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantTheme
{
    internal sealed class UpdateTenantThemeCommandHandler(
        ITenantThemeService themeService,
        ILogger<UpdateTenantThemeCommandHandler> logger)
        : ICommandHandler<UpdateTenantThemeCommand>
    {
        public async Task<Result> Handle(UpdateTenantThemeCommand request, CancellationToken cancellationToken)
        {
            await themeService.UpdateThemeAsync(request.TenantId, request.Theme, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Tenant theme updated: {TenantId}", request.TenantId);

            return Result.Success();
        }
    }
}