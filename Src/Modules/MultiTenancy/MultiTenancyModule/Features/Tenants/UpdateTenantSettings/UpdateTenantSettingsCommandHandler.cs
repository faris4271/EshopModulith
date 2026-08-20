using MediatR;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.UpdateTenantSettings;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.UpdateTenantSettings
{
    internal sealed class UpdateTenantSettingsCommandHandler(
        ITenantSettingsService settingsService)
        : ICommandHandler<UpdateTenantSettingsCommand, TenantSettingsDto>
    {
        public async Task<Result<TenantSettingsDto>> Handle(UpdateTenantSettingsCommand request, CancellationToken cancellationToken)
        {
            var settings = await settingsService.UpdateTenantSettingsAsync(request.TenantId, request.Settings, cancellationToken).ConfigureAwait(false);
            return Result.Success(settings);
        }
    }
}