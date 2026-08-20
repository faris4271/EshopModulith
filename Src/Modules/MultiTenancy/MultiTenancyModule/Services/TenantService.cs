using Finbuckle.MultiTenant.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultiTenancyModule.Contract.Dtos;
using MultiTenancyModule.Contract.Services;
using MultiTenancyModule.Data;
using MultiTenancyModule.Provisioning;
using Shared.Contract.Exeption;
using Shared.Multitenancy;
using Shared.Persistence;
using Shared.Web.SmartTable;

namespace MultiTenancyModule.Services;

public class TenantService : ITenantService
{
    private readonly IMultiTenantStore<AppTenantInfo> _tenantStore;
    private readonly DatabaseOptions _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly TenantDbContext _dbContext;
    private readonly ITenantProvisioningService _provisioningService;
    private readonly TimeProvider _timeProvider;
    private readonly TenantBillingOptions _billingOptions;
    private readonly ILogger<TenantService> _logger;

    public TenantService(
        IMultiTenantStore<AppTenantInfo> tenantStore,
        IOptions<DatabaseOptions> config,
        IServiceProvider serviceProvider,
        TenantDbContext dbContext,
        ITenantProvisioningService provisioningService,
        TimeProvider timeProvider,
        IOptions<TenantBillingOptions> billingOptions,
        ILogger<TenantService> logger)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(billingOptions);
        _tenantStore = tenantStore;
        _config = config.Value;
        _serviceProvider = serviceProvider;
        _dbContext = dbContext;
        _provisioningService = provisioningService;
        _timeProvider = timeProvider;
        _billingOptions = billingOptions.Value;
        _logger = logger;
    }
    public async Task<string> ActivateAsync(string id, CancellationToken cancellationToken)
    {
        var tenant = await GetTenantInfoAsync(id, cancellationToken);

        if (tenant.IsActive)
        {
            throw new CustomException("Tenant is already active");

        }

        await _provisioningService.EnsureCanActivateAsync(id, cancellationToken);

        tenant.Activate();

        await _tenantStore.UpdateAsync(tenant);
        await RefreshTenantCacheAsync(tenant);

        return tenant.Id;
    }

    public Task<DateTime> AdjustValidityAsync(string id, DateTime validUpto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> CreateAsync(string id, string name, string? connectionString, string adminEmail, string? issuer, string planKey, DateTime validUpto, CancellationToken cancellationToken)
    {
        AppTenantInfo tenant = new(id, name, connectionString, adminEmail, issuer)
        {
            Plan = planKey,
            // Set ValidUpto directly to the plan term: SetValidity() forbids moving the date backward, and
            // the ctor seeds now+1mo, so it would reject a term computed from an earlier 'now'.
            ValidUpto = DateTime.SpecifyKind(validUpto, DateTimeKind.Utc),
        };

        await _tenantStore.AddAsync(tenant).ConfigureAwait(false);
        await RefreshTenantCacheAsync(tenant);

        return tenant.Id;
    }

    public async Task<string> DeactivateAsync(string id, CancellationToken cancellationToken = default)
    {
        var tenant = await GetTenantInfoAsync(id, cancellationToken);

        if (!tenant.IsActive)
        {
            throw new CustomException("Tenant is already inactive");
        }

        if (tenant.Id.Equals(MultitenancyConstants.Root.Id, StringComparison.OrdinalIgnoreCase))
        {
            throw new CustomException("The root tenant cannot be deactivated.");
        }
        int tenantCount = (await _tenantStore.GetAllAsync().ConfigureAwait(false)).Count(t => t.IsActive);

        if (tenantCount <= 1)
        {
            throw new CustomException("Cannot deactivate the last active tenant");
        }

        tenant.Deactivate();

        await _tenantStore.UpdateAsync(tenant);
        await RefreshTenantCacheAsync(tenant);

        return tenant.Id;
    }

    public async Task<bool> ExistsWithIdAsync(string id, CancellationToken cancellationToken = default) =>
         await _tenantStore.GetAsync(id).ConfigureAwait(false) is not null;

    public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default) =>
        (await _tenantStore.GetAllAsync().ConfigureAwait(false)).Any(t => t.Name == name);

    public async Task<AppTenantInfo?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await _tenantStore.GetAsync(id).ConfigureAwait(false);

    public async Task<SmartTableResult<TenantDto>> GetAllAsync(SmartTableParam smartTableParam, CancellationToken cancellationToken)
    {

        IQueryable<AppTenantInfo> tenants = _dbContext.TenantInfo;



        return tenants.ToSmartTableResult(smartTableParam, tenant => new TenantDto
        {

            Id = tenant.Id,
            Name = tenant.Name,
            ConnectionString = tenant.ConnectionString,
            ValidUpto = tenant.ValidUpto,
            AdminEmail = tenant.AdminEmail,
            IsActive = tenant.IsActive,
            Issuer = tenant.Issuer

        });

    }

    public async Task<TenantStatusDto> GetStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        var tenant = await GetTenantInfoAsync(id);

        var graceEnds = tenant.ValidUpto.AddDays(_billingOptions.GracePeriodDays);

        var now = _timeProvider.GetUtcNow();

        string expiryState = now switch
        {
            _ when now <= tenant.ValidUpto => "Active",
            _ when now <= graceEnds => "InGrace",
            _ => "Expired"
        };

        return new TenantStatusDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            IsActive = tenant.IsActive,
            ValidUpto = tenant.ValidUpto,
            HasConnectionString = !string.IsNullOrWhiteSpace(tenant.ConnectionString),
            AdminEmail = tenant.AdminEmail,
            Issuer = tenant.Issuer,
            Plan = tenant.Plan,
            GraceEndsUtc = graceEnds,
            ExpiryState = expiryState

        };
    }

    public async Task MigrateTenantAsync(AppTenantInfo tenant, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenant);

        foreach (var initializer in scope.ServiceProvider.GetServices<IDbInitializer>())
        {
            await initializer.MigrateAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task<(DateTime PeriodStartUtc, DateTime ValidUpto, bool PlanChanged)> RenewAsync(
     string id, string newPlanKey, int termMonths, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newPlanKey);

        var tenant = await GetTenantInfoAsync(id, cancellationToken).ConfigureAwait(false);
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        // Stack remaining time: renew from ValidUpto if still in the future, otherwise from now.
        var periodStart = DateTime.SpecifyKind(tenant.ValidUpto > now ? tenant.ValidUpto : now, DateTimeKind.Utc);
        var newValidUpto = DateTime.SpecifyKind(periodStart.AddMonths(termMonths), DateTimeKind.Utc);
        var planChanged = !string.Equals(tenant.Plan, newPlanKey, StringComparison.OrdinalIgnoreCase);

        tenant.SetValidity(newValidUpto);
        if (planChanged)
        {
            tenant.Plan = newPlanKey;
        }

        await _tenantStore.UpdateAsync(tenant).ConfigureAwait(false);
        await RefreshTenantCacheAsync(tenant).ConfigureAwait(false);

        return (periodStart, newValidUpto, planChanged);
    }
    public async Task SeedTenantAsync(AppTenantInfo tenant, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
            .MultiTenantContext = new MultiTenantContext<AppTenantInfo>(tenant);

        foreach (var seedData in scope.ServiceProvider.GetServices<IDbInitializer>())
        {
            await seedData.SeedAsync(cancellationToken).ConfigureAwait(false);
        }


    }

    private async Task<AppTenantInfo> GetTenantInfoAsync(string id, CancellationToken cancellationToken = default) =>
    await _tenantStore.GetAsync(id).ConfigureAwait(false)
        ?? throw new NotFoundException($"{typeof(AppTenantInfo).Name} {id} Not Found.");

    private async Task RefreshTenantCacheAsync(AppTenantInfo tenant)
    {
        var cacheStore = _serviceProvider.GetRequiredService<IMultiTenantStore<AppTenantInfo>>();

        await cacheStore.UpdateAsync(tenant).ConfigureAwait(false);
    }
}