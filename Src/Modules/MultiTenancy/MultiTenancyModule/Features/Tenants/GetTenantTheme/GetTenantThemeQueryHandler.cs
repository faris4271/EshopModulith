using MediatR;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Features.Tenants.GetTenantTheme;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;

namespace MultiTenancyModule.Features.Tenants.GetTenantTheme
{
    internal sealed class GetTenantThemeQueryHandler(
        ITenantThemeService themeService)
        : IQueryHandler<GetTenantThemeQuery, TenantThemeDto>,
          IQueryHandler<GetCurrentTenantThemeQuery, TenantThemeDto>,
          IQueryHandler<GetDefaultTenantThemeQuery, TenantThemeDto>
    {
        public async Task<Result<TenantThemeDto>> Handle(GetTenantThemeQuery request, CancellationToken cancellationToken)
        {
            var theme = await themeService.GetThemeAsync(request.TenantId, cancellationToken).ConfigureAwait(false);
            return Result.Success(theme);
        }

        public async Task<Result<TenantThemeDto>> Handle(GetCurrentTenantThemeQuery request, CancellationToken cancellationToken)
        {
            var theme = await themeService.GetCurrentTenantThemeAsync(cancellationToken).ConfigureAwait(false);
            return Result.Success(theme);
        }

        public async Task<Result<TenantThemeDto>> Handle(GetDefaultTenantThemeQuery request, CancellationToken cancellationToken)
        {
            var theme = await themeService.GetDefaultThemeAsync(cancellationToken).ConfigureAwait(false);
            return Result.Success(theme);
        }
    }
}