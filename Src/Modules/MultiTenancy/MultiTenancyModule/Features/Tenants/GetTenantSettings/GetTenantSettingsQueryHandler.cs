using MediatR;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.GetTenantSettings;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantSettings
{
    internal sealed class GetTenantSettingsQueryHandler(
        ITenantSettingsService settingsService)
        : IQueryHandler<GetTenantSettingsQuery, TenantSettingsDto>
    {
        public async Task<Result<TenantSettingsDto>> Handle(GetTenantSettingsQuery request, CancellationToken cancellationToken)
        {
            var settings = await settingsService.GetTenantSettingsAsync(request.TenantId, cancellationToken).ConfigureAwait(false);

            if (settings is null)
            {
                return Result.Failure<TenantSettingsDto>(Error.NotFound("TenantSettings.NotFound", $"Tenant settings for tenant '{request.TenantId}' not found."));
            }

            return Result.Success(settings);
        }
    }
}