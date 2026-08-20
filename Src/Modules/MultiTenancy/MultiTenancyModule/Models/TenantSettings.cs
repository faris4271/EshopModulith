using Shared.DDD;

namespace MultiTenancyModule.Models;

public class TenantSettings : EntityBase<Guid>, IHasTenant, IAuditableEntity
{
    public string TenantId { get; private set; } = default!;

    public string? CompanyName { get; set; }

    public string DefaultCurrency { get; set; } = "USD";

    public string DefaultLanguage { get; set; } = "en";

    public string TimeZone { get; set; } = "UTC";

    public bool EnableEmailNotifications { get; set; } = true;

    public bool EnableSmsNotifications { get; set; } = false;

    public string? LogoUrl { get; set; }

    public string PrimaryColor { get; set; } = "#2563EB";

    public string SecondaryColor { get; set; } = "#0F172A";

    public string? CustomSettings { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? LatestUpdatedOn { get; set; }

    public string? CreatedById { get; set; }

    public string? LatestUpdatedById { get; set; }

    private TenantSettings()
    {
    }

    public static TenantSettings Create(Guid tenantId, string? createdBy = null)
    {
        return new TenantSettings
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.ToString(),
            CreatedById = createdBy,
            CreatedOn = DateTimeOffset.UtcNow
        };
    }

    public void Update(string? modifiedBy)
    {
        LatestUpdatedOn = DateTimeOffset.UtcNow;
        LatestUpdatedById = modifiedBy;
    }
}
