using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Data;
using MultiTenancyModule.Models;
using Shared.Contract.Exeption;

namespace MultiTenancyModule.Services;

internal sealed class TenantSettingsService : ITenantSettingsService
{
    private readonly TenantDbContext _dbContext;
    private readonly ILogger<TenantSettingsService> _logger;

    public TenantSettingsService(TenantDbContext dbContext, ILogger<TenantSettingsService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TenantSettingsDto?> GetTenantSettingsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.TenantSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId.ToString(), cancellationToken)
            .ConfigureAwait(false);

        return entity is null ? null : Map(entity);
    }

    public async Task<TenantSettingsDto> UpdateTenantSettingsAsync(Guid tenantId, UpdateTenantSettingsDto dto, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = await _dbContext.TenantSettings
            .FirstOrDefaultAsync(x => x.TenantId == tenantId.ToString(), cancellationToken)
            .ConfigureAwait(false);

        if (entity is null)
        {
            entity = TenantSettings.Create(tenantId);
            await _dbContext.TenantSettings.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        entity.CompanyName = dto.CompanyName ?? entity.CompanyName;
        entity.DefaultCurrency = dto.DefaultCurrency ?? entity.DefaultCurrency;
        entity.DefaultLanguage = dto.DefaultLanguage ?? entity.DefaultLanguage;
        entity.TimeZone = dto.TimeZone ?? entity.TimeZone;
        entity.EnableEmailNotifications = dto.EnableEmailNotifications ?? entity.EnableEmailNotifications;
        entity.EnableSmsNotifications = dto.EnableSmsNotifications ?? entity.EnableSmsNotifications;
        entity.LogoUrl = dto.LogoUrl ?? entity.LogoUrl;
        entity.PrimaryColor = dto.PrimaryColor ?? entity.PrimaryColor;
        entity.SecondaryColor = dto.SecondaryColor ?? entity.SecondaryColor;
        entity.CustomSettings = dto.CustomSettings ?? entity.CustomSettings;
        entity.Update(null);

        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Updated tenant settings for tenant {TenantId}", tenantId);
        }

        return Map(entity);
    }

    private static TenantSettingsDto Map(TenantSettings entity)
    {
        return new TenantSettingsDto(
            entity.Id,
            Guid.Parse(entity.TenantId),
            entity.CompanyName,
            entity.DefaultCurrency,
            entity.DefaultLanguage,
            entity.TimeZone,
            entity.EnableEmailNotifications,
            entity.EnableSmsNotifications,
            entity.LogoUrl,
            entity.PrimaryColor,
            entity.SecondaryColor,
            entity.CustomSettings,
            entity.CreatedOn,
            entity.LatestUpdatedOn);
    }
}
